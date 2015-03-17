// --------------------------
// RP Hunger
// --------------------------

%error = ForceRequiredAddon("RP_Core");

if (%error == $Error::AddOn_Disabled)
{
	warn("RP_Hunger: RP_Core has been enabled.");
}
if (%error == $Error::AddOn_NotFound)
{
	error("RP_Hunger: RP_Core is missing and is required by this Add-On.");
}
// Final check
else if (isFunction(RPModExist) && isFunction(CreateRPDB))
{
	// Register mod
	RPRegMod("Hunger");
	
	// Run scripts
	exec("./commands.cs");
	exec("./events.cs");
	exec("./game.cs");
	exec("./prefs.cs");
	exec("./user.cs");
}
