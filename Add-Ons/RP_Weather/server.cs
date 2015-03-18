// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// Project: RP_Weather
// Author: JJstorm
// Description: Weather For RP_Core
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// Table of Contents
// 1. Check For RP
// 2. Execute
// 
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
// 1. Check For RP
// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-

%error = ForceRequiredAddon("RP_Core");

if (%error == $Error::AddOn_Disabled)
{
	warn("RP_Weather: RP_Core Needed For This Addon Please, Enable It");
}
if (%error == $Error::AddOn_NotFound)
{
	error("RP_Weather: RP_Core is missing and is required by this Add-On.");
}
else if (isFunction(RPModExist) && isFunction(CreateRPDB))
{
	%error2 = ForceRequiredAddon("RP_Time");
	
	if (%error2 == $Error::AddOn_Disabled)
	{
		warn("RP_Weather: RP_Time Needed For This Addon Please, Enable It");
	}
	if (%error2 == $Error::AddOn_NotFound)
	{
		error("RP_Weather: RP_Time is missing and is required by this Add-On.");
	}
	else
	{
		// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
		// 2. Execute
		// -=-=--=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-=-
		
		// Register mod
		
		RPRegMod("RP_Weather");
			
		// Inject RP Weather
		exec("./game.cs");
		exec("./commands.cs");
		//exec("./datablocks.cs");
		
		// Load Extra
		exec("./initload.cs");
	}
	

}
