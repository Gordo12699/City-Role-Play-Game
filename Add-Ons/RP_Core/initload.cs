// --------------------------
// RP Core :: Init Load
// Info: This is where everything
// that have to be loaded directly goes
// --------------------------

// Standard
exec("./prefs.cs");

// Reload preferences
LoadPreferences();

// Create RTB Preferences
if (isFile("Add-Ons/System_ReturnToBlockland/server.cs") && !$RP::Core::RTB)
{
	if (!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	$RP::Core::RTB = true;
	
	// Server
	RTB_registerPref("Local Chat Distance", "RP Content", "$RP::pref::server::visibleDistance", "int 10 200", "RP Core", $RP::pref::server::visibleDistance, false, false, "");
	RTB_registerPref("Local Chat", "RP Content", "$RP::pref::server::localChat", "bool", "RP Core", $RP::pref::server::localChat, false, false, "");
	RTB_registerPref("Stats", "RP Content", "$RP::pref::server::stats", "bool", "RP Core", $RP::pref::server::stats, false, false, "");
	RTB_registerPref("Caps-Filter", "RP Content", "$RP::pref::server::capsFilter", "bool", "RP Core", $RP::pref::server::capsFilter, false, false, "");
	RTB_registerPref("Report Command", "RP Content", "$RP::pref::server::report", "bool", "RP Core", $RP::pref::server::report, false, false, "");
	RTB_registerPref("Suggest Command", "RP Content", "$RP::pref::server::suggest", "bool", "RP Core", $RP::pref::server::suggest, false, false, "");
	RTB_registerPref("Advanced Commands", "RP Content", "$RP::pref::server::rankAdvanceCommands", "list Noone 0 Host 1 SuperAdmins 2 Admins 3", "RP Core", $RP::pref::server::rankAdvanceCommands, false, true, "");
	
	// User
	RTB_registerPref("Status Bar Update Speed", "RP Content", "$RP::pref::user::updateInterval", "int 1 10", "RP Core", $RP::pref::user::updateInterval, false, true, "");
	RTB_registerPref("Amount Tools", "RP Content", "$RP::pref::user::amountTools", "int 1 10", "RP Core", $RP::pref::user::amountTools, true, true, "");
	RTB_registerPref("Last Action Limit", "RP Content", "$RP::pref::user::lastActionTimer", "int 1 10", "RP Core", $RP::pref::user::lastActionTimer, false, true, "");
	RTB_registerPref("Stop Activate Timer", "RP Content", "$RP::pref::user::stopActivate", "int 10 40", "RP Core", $RP::pref::user::stopActivate, false, true, "");
	RTB_registerPref("Unlock player scaling", "RP Content", "$RP::pref::user::playerScaling", "bool", "RP Core", $RP::pref::user::playerScaling, false, true, "");
	RTB_registerPref("Player Scale", "RP Content", "$RP::pref::user::playerScale", "string 16", "RP Core", $RP::pref::user::playerScale, false, true, "");
	RTB_registerPref("Player Model", "RP Content", "$RP::pref::user::playerModel", "string 32", "RP Core", $RP::pref::user::playerModel, false, true, "");

	// Misc
	RTB_registerPref("Can Jet", "RP Content", "$RP::pref::canJet", "bool", "RP Core", $RP::pref::canJet, true, true, "");
	RTB_registerPref("Minimum Jet Energy", "RP Content", "$RP::pref::minJetEnergy", "bool", "RP Core", $RP::pref::minJetEnergy, true, true, "");
	RTB_registerPref("Jet Energy Drain", "RP Content", "$RP::pref::jetEnergyDrain", "bool", "RP Core", $RP::pref::jetEnergyDrain, true, true, "");
}

// Clamp value
$RP::pref::user::amountTools = ($RP::pref::user::amountTools > 10) ? 10 : (($RP::pref::user::amountTools < 1) ? 1 : $RP::pref::user::amountTools);

datablock PlayerData(RP_Player : PlayerStandardArmor)
{
	minJetEnergy = ($RP::pref::canJet) ? $RP::pref::minJetEnergy : 0;
	jetEnergyDrain = ($RP::pref::canJet) ? $RP::pref::jetEnergyDrain : 0;
	canJet = ($RP::pref::canJet) ? 1 : 0;
	
	//maxHealth = 0;
	rechargeRate = 0.0;
	
	maxTools = $RP::pref::user::amountTools;
	maxWeapons = $RP::pref::user::amountTools;
	
	uiName = "RP Player";
	
	//showEnergyBar = true;
};
