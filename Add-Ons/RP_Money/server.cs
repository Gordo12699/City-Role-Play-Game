// --------------------------
// RP Money
// --------------------------

%errorA = ForceRequiredAddon("RP_Core");

if (%errorA == $Error::AddOn_Disabled)
{
	warn("RP_Money: RP_Core has been enabled.");
}
if (%errorA == $Error::AddOn_NotFound)
{
	error("RP_Money: RP_Core is missing and is required by this Add-On.");
}
// Final check
else if (isFunction(RPModExist) && isFunction(CreateRPDB))
{
	// Register mod
	RPRegMod("Money");
	
	// Run scripts
	exec("./system.cs");
	exec("./game.cs");
	exec("./user.cs");
	exec("./commands.cs");
	exec("./bricks.cs");
	exec("./events.cs");
	exec("./cash.cs");

	// And the last stuff
	exec("./initload.cs");
}

//we need the timed bomb add-on for this, so force it to load
%errorB = ForceRequiredAddOn("Weapon_TimedBomb");

if(%errorB == $Error::AddOn_NotFound)
{
   //we don't have the SWAT Armour, so we're screwed
   error("ERROR: RP_Money - required add-on Weapon_TimedBomb not found");
}

//SWAT Armour
%errorC = ForceRequiredAddOn("Item_Armor_SWAT");

if(%errorC == $Error::AddOn_NotFound)
{
   //we don't have the SWAT Armour, so we're screwed
   error("ERROR: RP_Money - required add-on Item_Armor_SWAT not found");
}
