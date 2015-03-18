registerOutputEvent(fxDTSBrick, ifRPWeather, "list Equals 0 NotEquals 1" TAB "list Clear 0 Rain 1 Snow 2", 1);

function fxDTSBrick::ifRPWeather(%brick, %RPWmode, %RPWeather, %client)
{

	//Set Currect Values
	if(%RPWeather == 0)
	{
		%RPW = "clear";
	}
	else if(%RPWeather == 1)
	{
		%RPW = "rain";
	}
	else if(%RPWeather == 2)
	{
		%RPW = "snow";
	}


	// Equals And Not Equals
	switch (%RPWmode)
	{
		// Equals
		case 0:
			if ($RP::pref::CurrWeather $= %RPW) {
				%brick.onRPTrue(%client);
			}
			else
			{
				%brick.onRPFalse(%client);
			}
		// Not Equals
		case 1:
			if ($RP::pref::CurrWeather !$= %RPW) {
				%brick.onRPTrue(%client);
			}
			else
			{
				%brick.onRPFalse(%client);
			}
	}
}
