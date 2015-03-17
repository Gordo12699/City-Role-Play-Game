// --------------------------
// RP Crime :: Events
// --------------------------

// Check if job is true
function fxDTSBrick::ifLaw(%brick, %mode, %client)
{
	switch (%mode)
	{
		// Equals
		case 1:
			if ($RP::job::law[%client.RPData.value["jobID"]])
			{
				%brick.onRPTrue(%client);
			}
			else
			{
				%brick.onRPFalse(%client);
			}
			
			// Equals
		case 0:
			if ($RP::job::law[%client.RPData.value["jobID"]])
			{
				%brick.onRPFalse(%client);
				
			}
			else
			{
				%brick.onRPTrue(%client);
			}
	}
}

registerOutputEvent(fxDTSBrick, "ifLaw", "bool");

if (isPackage(RP_Crime_Events))
	deactivatePackage(RP_Crime_Events);

package RP_Crime_Events
{
	// Transfering money to brick
	function fxDTSBrick::transferMoneyToBrick(%brick, %service, %amount, %client)
	{
		if (!%client.isInRP())
			return;
		%money1 = %client.RPData.value["money"];
		Parent::transferMoneyToBrick(%brick, %service, %amount, %client);
		%money2 = %client.RPData.value["money"];
		%stolen = %client.RPData.value["stolenMoney"];
		if (%stolen == 0 || %money1 == %money2)
			return;
		
		%clean = %money1 - %money2;
		%client.RPData.value["stolenMoney"] -= (%clean > %stolen) ? %stolen : %clean;
	}
};
RPRegPackage(RP_Crime_Events);
