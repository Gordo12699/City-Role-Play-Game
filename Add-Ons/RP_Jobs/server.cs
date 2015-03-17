// --------------------------
// RP Jobs
// --------------------------

%error = ForceRequiredAddon("RP_Core");

if (%error == $Error::AddOn_Disabled)
{
	warn("RP_Jobs: RP_Core has been enabled.");
}
if (%error == $Error::AddOn_NotFound)
{
	error("RP_Jobs: RP_Core is missing and is required by this Add-On.");
}
// Final check
else if (isFunction(RPModExist) && isFunction(CreateRPDB))
{
	// Register mod
	RPRegMod("Jobs");
	
	// Run scripts
	exec("./game.cs");
	exec("./bricks.cs");
	exec("./commands.cs");
	exec("./jobs.cs");
	exec("./events.cs");
	exec("./user.cs");
	//exec("./clothes.cs"); //Not Fixed - Random parts of the player avatar will still display but with the color white (helmet)

	// And the last stuff
	exec("./initload.cs");
}
