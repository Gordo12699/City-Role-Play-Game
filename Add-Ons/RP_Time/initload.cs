// --------------------------
// RP Time :: Init Load
// Info: This is where everything
// that have to be loaded directly goes
// --------------------------

// Standard
exec("./prefs.cs");

// Reload preferences
LoadPreferences();

// Create RTB Preferences
if (isFile("Add-Ons/System_ReturnToBlockland/server.cs") && !$RP::Time::RTB)
{
	if (!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	$RP::Time::RTB = true;
	
	// Game
	RTB_registerPref("Minutes to Tick", "RP Content", "$RP::pref::game::tick", "int 10 40", "RP Time", $RP::pref::server::tick, false, true, "");
	RTB_registerPref("AM/PM", "RP Content", "$RP::pref::game::am_pm", "bool", "RP Time", $RP::pref::server::am_pm, false, true, "");

	// User
	RTB_registerPref("Hourly Experience", "RP Content", "$RP::pref::user::tickExp", "int 0 10", "RP Time", $RP::pref::user::tickExp, false, true, "");

	// Misc
	RTB_registerPref("Date Hour", "RP Content", "$RP::pref::timeHour", "int 1 24", "RP Time", $RP::pref::timeHour, false, true, "");
	RTB_registerPref("Date Day", "RP Content", "$RP::pref::timeDay", "int 1 31", "RP Time", $RP::pref::timeDay, false, true, "");
	RTB_registerPref("Date Month", "RP Content", "$RP::pref::timeMonth", "int 1 12", "RP Time", $RP::pref::timeMonth, false, true, "");
	RTB_registerPref("Date Year", "RP Content", "$RP::pref::timeYear", "int 0 100000", "RP Time", $RP::pref::timeYear, false, true, "");
}
