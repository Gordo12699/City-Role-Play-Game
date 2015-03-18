// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// RP Weather :: Init Load
// Info: Weather Prefs And RTB Prefs
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

// Standard Prefs
exec("./prefs.cs");

// Reload Preferences
LoadPreferences();

// Load Weather Event
exec("./events.cs");

// RTB Prefs
if(isFile("Add-Ons/System_ReturnToBlockland/server.cs")) {
	if(!$RTB::RTBR_ServerControl_Hook)
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
		
	// RTB Prefs
	RTB_registerPref("Radio Name","RP Content","$RP::pref::radioname","string 4","RP_Weather",$RP::pref::radioname, 0, 1, "");
	RTB_registerPref("Radio Frequency","RP Content","$RP::pref::radiofreq","string 20","RP_Weather",$RP::pref::radiofreq, 0, 1, "");
	RTB_registerPref("RP_Weather | Effect Sky","RP Content","$RP::pref::altersky","bool","RP_Weather",$RP::pref::altersky, 0, 1, "");
	RTB_registerPref("RP_Weather | Effect Sun","RP Content","$RP::pref::altersun","bool","RP_Weather",$RP::pref::altersun, 0, 1, "");
	RTB_registerPref("RP_Weather | Effect SunFX","RP Content","$RP::pref::altersunfx","bool","RP_Weather",$RP::pref::altersunfx, 0, 1, "");
	RTB_registerPref("RP_Weather | Weather","RP Content","$RP::pref::Weather","bool","RP_Weather",$RP::pref::Weather, 0, 1, "");
	
	// Rain Chances
	RTB_registerPref("Chance Of Rain In January","RP Content","$RP::pref::RainMonth1","int 0 100","RP_Weather",$RP::pref::RainMonth1, 0, 1, "");
	RTB_registerPref("Chance Of Rain In February","RP Content","$RP::pref::RainMonth2","int 0 100","RP_Weather",$RP::pref::RainMonth2, 0, 1, "");
	RTB_registerPref("Chance Of Rain In March","RP Content","$RP::pref::RainMonth3","int 0 100","RP_Weather",$RP::pref::RainMonth3, 0, 1, "");
	RTB_registerPref("Chance Of Rain In April","RP Content","$RP::pref::RainMonth4","int 0 100","RP_Weather",$RP::pref::RainMonth4, 0, 1, "");
	RTB_registerPref("Chance Of Rain In May","RP Content","$RP::pref::RainMonth5","int 0 100","RP_Weather",$RP::pref::RainMonth5, 0, 1, "");
	RTB_registerPref("Chance Of Rain In June","RP Content","$RP::pref::RainMonth6","int 0 100","RP_Weather",$RP::pref::RainMonth6, 0, 1, "");
	RTB_registerPref("Chance Of Rain In July","RP Content","$RP::pref::RainMonth7","int 0 100","RP_Weather",$RP::pref::RainMonth7, 0, 1, "");
	RTB_registerPref("Chance Of Rain In August","RP Content","$RP::pref::RainMonth8","int 0 100","RP_Weather",$RP::pref::RainMonth8, 0, 1, "");
	RTB_registerPref("Chance Of Rain In September","RP Content","$RP::pref::RainMonth9","int 0 100","RP_Weather",$RP::pref::RainMonth9, 0, 1, "");
	RTB_registerPref("Chance Of Rain In October","RP Content","$RP::pref::RainMonth10","int 0 100","RP_Weather",$RP::pref::RainMonth10, 0, 1, "");
	RTB_registerPref("Chance Of Rain In November","RP Content","$RP::pref::RainMonth11","int 0 100","RP_Weather",$RP::pref::RainMonth11, 0, 1, "");
	RTB_registerPref("Chance Of Rain In December","RP Content","$RP::pref::RainMonth12","int 0 100","RP_Weather",$RP::pref::RainMonth12, 0, 1, "");

	// Snow Chances
	RTB_registerPref("Chance Of Snow In January","RP Content","$RP::pref::SnowMonth1","int 0 100","RP_Weather",$RP::pref::SnowMonth1, 0, 1, "");
	RTB_registerPref("Chance Of Snow In February","RP Content","$RP::pref::SnowMonth2","int 0 100","RP_Weather",$RP::pref::SnowMonth2, 0, 1, "");
	RTB_registerPref("Chance Of Snow In March","RP Content","$RP::pref::SnowMonth3","int 0 100","RP_Weather",$RP::pref::SnowMonth3, 0, 1, "");
	RTB_registerPref("Chance Of Snow In April","RP Content","$RP::pref::SnowMonth4","int 0 100","RP_Weather",$RP::pref::SnowMonth4, 0, 1, "");
	RTB_registerPref("Chance Of Snow In May","RP Content","$RP::pref::SnowMonth5","int 0 100","RP_Weather",$RP::pref::SnowMonth5, 0, 1, "");
	RTB_registerPref("Chance Of Snow In June","RP Content","$RP::pref::SnowMonth6","int 0 100","RP_Weather",$RP::pref::SnowMonth6, 0, 1, "");
	RTB_registerPref("Chance Of Snow In July","RP Content","$RP::pref::SnowMonth7","int 0 100","RP_Weather",$RP::pref::SnowMonth7, 0, 1, "");
	RTB_registerPref("Chance Of Snow In August","RP Content","$RP::pref::SnowMonth8","int 0 100","RP_Weather",$RP::pref::SnowMonth8, 0, 1, "");
	RTB_registerPref("Chance Of Snow In September","RP Content","$RP::pref::SnowMonth9","int 0 100","RP_Weather",$RP::pref::SnowMonth9, 0, 1, "");
	RTB_registerPref("Chance Of Snow In October","RP Content","$RP::pref::SnowMonth10","int 0 100","RP_Weather",$RP::pref::SnowMonth10, 0, 1, "");
	RTB_registerPref("Chance Of Snow In November","RP Content","$RP::pref::SnowMonth11","int 0 100","RP_Weather",$RP::pref::SnowMonth11, 0, 1, "");
	RTB_registerPref("Chance Of Snow In December","RP Content","$RP::pref::SnowMonth12","int 0 100","RP_Weather",$RP::pref::SnowMonth12, 0, 1, "");
}
