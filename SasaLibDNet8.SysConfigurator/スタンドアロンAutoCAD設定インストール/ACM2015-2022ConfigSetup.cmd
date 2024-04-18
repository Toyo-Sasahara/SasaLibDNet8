ECHO OFF
pushd %~dp0

del .\*.log
"..\bin\Debug\SasaLib.SysConfigurator.exe" ".\AM2022テンプレート・図枠・表題欄など取得.Conf" "."
pause
"..\bin\Debug\SasaLib.SysConfigurator.exe" ".\AM2020テンプレート・図枠・表題欄など取得.Conf" "."
pause
"..\bin\Debug\SasaLib.SysConfigurator.exe" ".\AM2015テンプレート・図枠・表題欄など取得.Conf" "."

popd
