// --------------------------
// RP Jobs :: Events
// --------------------------

// Check if job is true
function fxDTSBrick::ifJob(%brick, %mode, %job, %client)
{
	%jobID = %client.RPData.value["jobID"];
	switch (%mode)
	{
		// Equals
		case 0:
			if (%job == %jobID)
				%brick.onRPTrue(%client);
			else
				%brick.onRPFalse(%client);
		// Not Equals
		case 1:
			if (%job != %jobID)
				%brick.onRPTrue(%client);
			else
				%brick.onRPFalse(%client);
	}
}
