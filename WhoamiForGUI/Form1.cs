using System.DirectoryServices.AccountManagement;
using System.Security.Principal;

namespace WhoamiForGUI
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            txtUser.Text = $"{Environment.UserDomainName}\\{Environment.UserName}";  // 既定値として現在ユーザー
        }

        // ルートロード
        private void MainForm_Load(object sender, EventArgs e)
        {
        }

        /// <summary>ドメイン参加ならドメイン、非参加ならローカル SAM に接続</summary>
        private static PrincipalContext CreatePrincipalContext()
        {
            try
            {
                // ドメイン参加済みか確認
                return new PrincipalContext(ContextType.Domain);
            }
            catch
            {
                // 非ドメイン環境 or 接続 NG → ローカル SAM
                return new PrincipalContext(ContextType.Machine);
            }
        }

        /// <summary>現在のアクセストークンに含まれるグループ名一覧</summary>
        private static List<string> GetTokenGroups()
        {
            using var id = WindowsIdentity.GetCurrent();
            return id.Groups?
                     .Select(sid =>
                     {
                         try { return sid.Translate(typeof(NTAccount)).Value; }
                         catch { return sid.Value; }
                     })
                     .Distinct(StringComparer.OrdinalIgnoreCase)
                     .ToList()
                   ?? new List<string>();
        }

        /// <summary>
        /// 再帰的にネストされたグループを探索し TreeNode を追加
        /// </summary>
        private static void ExpandNested(PrincipalContext ctx, string groupName,
                                         TreeNode parentNode, HashSet<string> visited)
        {
            // ループ防止
            if (!visited.Add(groupName)) return;

            using var gp = GroupPrincipal.FindByIdentity(ctx, groupName);
            if (gp == null) return;            // 名前解決できない (ローカル SID など)

            foreach (var member in gp.GetMembers())
            {
                if (member is not GroupPrincipal childGroup) continue;

                var childNode = parentNode.Nodes.Add(childGroup.SamAccountName ?? childGroup.Name);
                ExpandNested(ctx, childGroup.SamAccountName ?? childGroup.Name,
                             childNode, visited);
            }
        }

        //──────────────────────────────────────────────
        // 1. ボタン押下 → 非同期でグループ検索
        //──────────────────────────────────────────────
        private async void BtnLoad_Click(object? sender, EventArgs e)
        {
            string target = txtUser.Text.Trim();
            if (string.IsNullOrWhiteSpace(target))
            {
                MessageBox.Show("ユーザー名を入力してください。", Text,
                                MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            btnLoad.Enabled = false;
            lblInfo.Text = "読み込み中…";

            try
            {
                await Task.Run(() => BuildTreeForUser(target));
                lblInfo.Text = $"ユーザー [{target}] のグループを表示しました";
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                lblInfo.Text = "読み込みに失敗しました";
            }
            finally
            {
                btnLoad.Enabled = true;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GetTokenGroups_button_Click(object sender, EventArgs e)
        {
            // ① ログオン・トークンに含まれるグループ SID を名前へ変換
            var directGroups = GetTokenGroups();

            // ② Active Directory / ローカル SAM へ接続
            using var ctx = CreatePrincipalContext();

            // ③ TreeView に展開
            tvGroups.BeginUpdate();
            tvGroups.Nodes.Clear();

            foreach (var g in directGroups.OrderBy(n => n))
            {
                var root = tvGroups.Nodes.Add(g);
                ExpandNested(ctx, g, root, new HashSet<string>(StringComparer.OrdinalIgnoreCase));
            }

            tvGroups.EndUpdate();
            lblCount.Text = $"ルートグループ数: {directGroups.Count}";
            tvGroups.ExpandAll();

        }

        /// <summary>
        /// 指定ユーザー→ルートグループ→下位グループを再帰展開
        /// </summary>
        /// <param name="samOrUpn"></param>
        /// <exception cref="ApplicationException"></exception>
        private void BuildTreeForUser(string samOrUpn)
        {
            using PrincipalContext ctx = CreateContextForUser(samOrUpn);
            using UserPrincipal? user = UserPrincipal.FindByIdentity(ctx, samOrUpn);

            if (user is null)
                throw new ApplicationException("ユーザーが見つかりませんでした。");

            var authGroups = user.GetAuthorizationGroups()
                                 .OfType<GroupPrincipal>()
                                 .OrderBy(g => g.SamAccountName ?? g.Name)
                                 .ToList();

            Invoke((Delegate)(() =>
            {
                tvGroups.BeginUpdate();
                tvGroups.Nodes.Clear();

                var visited = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
                foreach (var gp in authGroups)
                {
                    TreeNode root = tvGroups.Nodes.Add(Display(gp));
                    if (ExpandUpward_radioButton.Checked)
                        ExpandUpward(gp, root, visited);
                    else if (ExpandDownward_radioButton.Checked)
                        ExpandDownward(gp, root, visited);
                }

                tvGroups.ExpandAll();
                tvGroups.EndUpdate();
            }));
        }

        private static void ExpandUpward(GroupPrincipal gp, TreeNode node, HashSet<string> visited)
        {
            if (gp?.Sid is null || !visited.Add(gp.Sid.Value))
                return;

            foreach (var parent in gp.GetGroups().OfType<GroupPrincipal>())
            {
                TreeNode parentNode = node.Nodes.Add(Display(parent));
                ExpandUpward(parent, parentNode, visited);
            }
        }

        //──────────────────────────────────────────────
        // 3. 下位グループ (members) を再帰的にたどる
        //──────────────────────────────────────────────
        private static void ExpandDownward(GroupPrincipal gp, TreeNode node, HashSet<string> visited)
        {
            if (gp?.Sid is null || !visited.Add(gp.Sid.Value)) return;  // 巡回 or null → 終了

            foreach (var member in gp.GetMembers(false))   // false = 直下メンバーのみ
            {
                if (member is not GroupPrincipal childGp) continue;     // ユーザーや他オブジェクトは除外

                TreeNode childNode = node.Nodes.Add(Display(childGp));
                ExpandDownward(childGp, childNode, visited);
            }
        }

        //──────────────────────────────────────────────
        // 4. ユーザー文字列から最適コンテキスト生成（ドメイン or SAM）
        //──────────────────────────────────────────────
        private static PrincipalContext CreateContextForUser(string user)
        {
            string? domain = user.Contains('@') ? user.Split('@')[1]
                         : user.Contains('\\') ? user.Split('\\')[0]
                         : null;

            try
            {
                return domain is not null ? new PrincipalContext(ContextType.Domain, domain)
                                           : new PrincipalContext(ContextType.Domain);
            }
            catch
            {
                return new PrincipalContext(ContextType.Machine); // ドメイン未接続 → ローカル SAM
            }
        }

        //──────────────────────────────────────────────
        // 5. 表示名ユーティリティ
        //──────────────────────────────────────────────
        // private static string Display(GroupPrincipal gp) => gp.SamAccountName ?? gp.Name ?? gp.DistinguishedName ?? "(unknown)";


        //private static string Display(GroupPrincipal gp)
        //{
        //    if (gp is null) return "(unknown)";

        //    string name = gp.SamAccountName ?? gp.Name ?? gp.DistinguishedName ?? "(unknown)";
        //    string groupType = gp.ContextType switch
        //    {
        //        ContextType.Machine => "[ローカル]",
        //        ContextType.Domain => "[ドメイン]",
        //        _ => "[不明]"
        //    };

        //    SIDのプレフィックスでさらに組み込みグループ識別可能（例: S - 1 - 5 - 32 は組み込みローカルグループ）
        //    if (gp.Sid?.Value.StartsWith("S-1-5-32") == true)
        //    {
        //        groupType = "[組み込みローカル]";
        //    }

        //    return $"{groupType} {name}";
        //}


        private static string Display(GroupPrincipal gp)
        {
            if (gp is null) return "(unknown)";

            string name = gp.SamAccountName ?? gp.Name ?? gp.DistinguishedName ?? "(unknown)";
            string sid = gp.Sid?.Value ?? string.Empty;

            string groupType;

            if (sid.StartsWith("S-1-5-32"))  // 組み込みローカルグループ
            {
                groupType = "[組み込みローカル]";
            }
            else if (sid == "S-1-5-11")      // Authenticated Users
            {
                groupType = "[組み込み]";
            }
            else if (gp.ContextType == ContextType.Machine)
            {
                groupType = "[ローカル]";
            }
            else if (gp.ContextType == ContextType.Domain)
            {
                groupType = "[ドメイン]";
            }
            else
            {
                groupType = "[不明]";
            }

            return $"{groupType} {name}";
        }

    }

}

