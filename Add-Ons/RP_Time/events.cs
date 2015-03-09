// --------------------------
// RP Time :: Events
// --------------------------

// Output
registerOutputEvent(fxDTSBrick, ifRPDate, "int -1 60 -1" TAB "int 0 24 0" TAB "int 0 31 0" TAB "int 0 12 0", 1);

// Control date
function fxDTSBrick::ifRPDate(%brick, %minute, %hour, %day, %month, %client)
{
	%test = true;
	// Minute
	if (%minute != -1)
		if (%minute != getMinutes())
			%test = false;
	// Hour
	if (%hour != 0)
		if (%hour != $RP::pref::timeHour)
			%test = false;
	// Day
	if (%day != 0)
		if (%day != $RP::pref::timeDay)
			%test = false;
	// Month
	if (%month != 0)
		if (%month != $RP::pref::timeMonth)
			%test = false;
	// And last check
	if (%test)
		%brick.onRPTrue(%client);
	else
		%brick.onRPFalse(%client);
}
