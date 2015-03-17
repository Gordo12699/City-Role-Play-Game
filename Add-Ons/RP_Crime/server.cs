// --------------------------
// RP Crime
// --------------------------

%error = ForceRequiredAddon("RP_Core");

if (%error == $Error::AddOn_Disabled)
{
	warn("RP_Crime: RP_Core has been enabled.");
}
if (%error == $Error::AddOn_NotFound)
{
	error("RP_Crime: RP_Core is missing and is required by this Add-On.");
}
// Final check
else if (isFunction(RPModExist) && isFunction(CreateRPDB))
{
	// Register mod
	RPRegMod("Crime");
	
	// Run scrits
	exec("./game.cs");
	exec("./commands.cs");
	exec("./bricks.cs");
	exec("./user.cs");
	exec("./crime.cs");
	exec("./events.cs");
	
	// And the last stuff
	exec("./initload.cs");
}
