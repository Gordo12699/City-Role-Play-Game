// --------------------------
// RP Core :: Game
// --------------------------

// Boot up RP
function BootRP()
{
	if ($RP::activated)
		return;
	
	// Change quota for smoother playing
	if ($Game::DefaultQuota::Schedules < 100)
		$Game::DefaultQuota::Schedules = 100;
	if ($Game::MaxQuota::Schedules < 2000)
		$Game::MaxQuota::Schedules = 2000;
	if ($Pref::Server::Quota::Schedules < 100)
		$Pref::Server::Quota::Schedules = 100;
	
	// Create database
	CreateRPDB();
	
	// Create values
	RPDB.addValue("name", ""); // Will later be filled with player name
	RPDB.addValue("tools", ""); // Avoid the tools to not be added
	RPDB.addValue("exp", 0);
	RPDB.saveDefs();
	
	// Add stats
	RP_AddStat("Name", "name", 0);
	RP_AddStat("Exp", "exp", $RP::pref::user::displayExp);
	
	// Start Minigame
	CreateMiniGame();
	
	// Users joining the RP
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
		ClientGroup.getObject(%i).EnterRPGame();
	$RP::activated = true;
	
	CRPG_AssembleRealEstateEvents();
}

// Shutdown the RP
function ShutRP()
{
	if (!$RP::activated)
		return;
	
	$RP::activated = false;
	
	// Leaving RP
	%count = ClientGroup.getCount();
	for (%i = 0; %i < %count; %i++)
		ClientGroup.getObject(%i).LeaveRPGame();
	
	if (isObject(RPDB))
		RPDB.delete();
	
	RemoveMiniGame();
	
	// Exporting variables
	export("$RP::pref::*", "config/server/RP_Core/prefs.cs");
	
	// Removing temporary variables
	deleteVariables("$RP::setting*");
	deleteVariables("$RP::stats*");
}

// Create dababase
function CreateRPDB()
{
	if (isObject(RPDB))
		return;
	new ScriptObject(RPDB)
	{
		class = Saver;
		folder = "config/server/RP_Core/saves/";
		file = "config/server/RP_Core/listID.dat";
		defFile = "config/server/RP_Core/default.dat";
		saveExt = "sav";
	};
}

// Load preferences
function LoadPreferences()
{
	// First time
	if (!isFile("config/server/RP_Core/prefs.cs"))
		export("$RP::pref::*", "config/server/RP_Core/prefs.cs");
	
	// User defined
	exec("config/server/RP_Core/prefs.cs");
}
