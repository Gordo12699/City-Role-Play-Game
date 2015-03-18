// --------------------------
// RP Resources
// --------------------------

%error = ForceRequiredAddon("RP_Core");

if (%error == $Error::AddOn_Disabled)
{
	warn("RP_Resources: RP_Core has been enabled.");
}
if (%error == $Error::AddOn_NotFound)
{
	error("RP_Resources: RP_Core is missing and is required by this Add-On.");
}
// Final check
else if (isFunction(RPModExist) && isFunction(CreateRPDB))
{
	// Register mod
	RPRegMod("Resources");
	
	// Run scripts
	exec("./bricks.cs");
	exec("./game.cs");
	exec("./resources.cs");

	// And the last stuff
	exec("./initload.cs");
}
