// --------------------------
// RP Jobs :: Init Load
// Info: This is where everything
// that have to be loaded directly goes
// --------------------------

// Standard
exec("./prefs.cs");

// Reload preferences
LoadPreferences();

// Load jobs
LoadJobs();

// Employment Office
exec("./bricks/employment.cs");

// Create spawns
CreateSpawns();

// Put this here due of that it needed the jobs
// Output
registerOutputEvent(fxDTSBrick, "ifJob", "list Equals 0 NotEquals 1" TAB EventJobList());

// Create RTB Preferences
if (isFile("Add-Ons/System_ReturnToBlockland/server.cs") && !$RP::Jobs::RTB)
{
	if (!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	$RP::Jobs::RTB = true;
	
	// Server
	RTB_registerPref("Auto Decline Promotion Time", "RP Content", "$RP::pref::server::autoDeclineTime", "int 10 40", "RP Jobs", $RP::pref::server::autoDeclineTime, false, true, "");

	// User
	RTB_registerPref("Promotion Experience", "RP Content", "$RP::pref::user::promoteExp", "int 1 100", "RP Jobs", $RP::pref::user::promoteExp, false, true, "");
}
