// --------------------------
// RP Resources :: Init Load
// Info: This is where everything
// that have to be loaded directly goes
// --------------------------

// Load bricks
LoadOre();
LoadGem();
LoadTree();
LoadFish();

// RP Money
if (RPModExist("Money") || $AddOn__RP_Money == 1)
	exec("./bricks/sell.cs");

// Load items
%standard = true;
if (isFile("Add-Ons/Tool_RPG.zip"))
{
	// Tool RPG by Lilboarder
	%error = ForceRequiredAddon("Tool_RPG");
	%standard = false;

	if(%error == $Error::AddOn_Disabled)
	{
		warn("RP_Resources: Tool_RPG has been enabled.");
	}
	else if(%error == $Error::AddOn_NotFound)
	{
		warn("RP_Resources: Tool_RPG does not exist. Going to default...");
		%standard = true;
	}
	
	if (!%standard)
	{
		exec("./items/rpgAxe.cs");
		exec("./items/rpgPickaxe.cs");
	}
}

if (%standard)
{
	exec("./items/axe.cs");
	exec("./items/pickaxe.cs");
}
//exec(isFile("Add-Ons/Tool_RPG/Weapon_rpgAxe.cs") ? "./items/rpgAxe.cs" : "./items/axe.cs");
//exec("./items/axe.cs");
//exec(isFile("Add-Ons/Tool_RPG/Weapon_rpgPickaxe.cs") ? "./items/rpgPickaxe.cs" : "./items/pickaxe.cs");
//exec("./items/pickaxe.cs");
exec("./items/rod.cs");

// Standard
exec("./prefs.cs");

// Reload preferences
LoadPreferences();

// Create RTB Preferences
if (isFile("Add-Ons/System_ReturnToBlockland/server.cs") && !$RP::Resources::RTB)
{
	if (!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	$RP::Resources::RTB = true;
	
	// Server
	RTB_registerPref("Experience Mining Ore", "RP Content", "$RP::pref::server::hardcoreExp", "int 1 5000", "RP Resources", $RP::pref::server::hardcoreExp, false, true, "");

	// Gain
	RTB_registerPref("Experience Gain Tree", "RP Content", "$RP::pref::gain::cutTree", "int 1 10", "RP Resources", $RP::pref::gain::cutTree, false, true, "");
	RTB_registerPref("Experience Gain Ore", "RP Content", "$RP::pref::gain::mineOre", "int 1 10", "RP Resources", $RP::pref::gain::mineOre, false, true, "");
	RTB_registerPref("Experience Gain Fish", "RP Content", "$RP::pref::gain::fishing", "int 1 10", "RP Resources", $RP::pref::gain::fishing, false, true, "");
	RTB_registerPref("Money Gain Tree", "RP Content", "$RP::pref::money::wood", "int 1 10", "RP Resources", $RP::pref::money::wood, false, true, "");
	RTB_registerPref("Money Gain Ore", "RP Content", "$RP::pref::money::ore", "int 1 10", "RP Resources", $RP::pref::money::ore, false, true, "");
	RTB_registerPref("Money Gain Fish", "RP Content", "$RP::pref::money::fishing", "int 1 10", "RP Resources", $RP::pref::money::fishing, false, true, "");

	// Misc
	RTB_registerPref("Ore Name", "RP Content", "$RP::pref::oreName", "string 16", "RP Resources", $RP::pref::oreName, false, true, "");
	RTB_registerPref("Wood Name", "RP Content", "$RP::pref::woodName", "string 16", "RP Resources", $RP::pref::woodName, false, true, "");
	RTB_registerPref("Fish Name", "RP Content", "$RP::pref::fishName", "string 16", "RP Resources", $RP::pref::fishName, false, true, "");
}
