// --------------------------
// RP Time :: Commands
// --------------------------

// Check date
function ServerCmdDate(%client)
{
	if (!$RP::activated)
		return;
	
	messageClient(%client, '', '\c6The date is: \c3%1\c3 %2 %3\c6%4 \c3%5\c6, \c3%6', 
	getClockTime($RP::pref::timeHour, $RP::pref::game::am_pm),
	GetDayOfWeek($RP::pref::timeYear, $RP::pref::timeMonth, $RP::pref::timeDay),
	$RP::pref::timeDay,
	getNumberExtra($RP::pref::timeDay),
	$Date::monthName[$RP::pref::timeMonth],
	$RP::pref::timeYear);
}

// Force a tick
function ServerCmdForceTick(%client)
{
	if (!$RP::activated)
		return;
	if (!%client.CheckPrefTrust($RP::pref::server::rankAdvanceCommands))
		return;
	
	messageAll('', '\c3%1\c6 forced an hour to go.', %client.name);
	RPTick();
}
