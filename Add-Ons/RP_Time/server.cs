// --------------------------
// RP Time
// --------------------------

%error = ForceRequiredAddon("RP_Core");

if (%error == $Error::AddOn_Disabled)
{
	warn("RP_Time: RP_Core has been enabled.");
}
if (%error == $Error::AddOn_NotFound)
{
	error("RP_Time: RP_Core is missing and is required by this Add-On.");
}
// Final check
else if (isFunction(RPModExist) && isFunction(CreateRPDB))
{
	// Register mod
	RPRegMod("Time");
	
	// Run scripts
	exec("./daycycles.cs");
	exec("./system.cs");
	exec("./scriptObject.cs");
	exec("./game.cs");
	exec("./commands.cs");
	exec("./events.cs");

	// And the last stuff
	exec("./initload.cs");
}
