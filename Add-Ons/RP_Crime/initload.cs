// --------------------------
// RP Crime :: Init Load
// Info: This is where everything
// that have to be loaded directly goes
// --------------------------

// Load bricks
exec("./bricks/bounty.cs");
exec("./bricks/police.cs");

// Load items
exec("./items/baton.cs");
exec("./items/lockpick.cs");
//exec("./items/jailbaton.cs");

// Standard
exec("./prefs.cs");

// Reload preferences
LoadPreferences();

// Create RTB Preferences
if (isFile("Add-Ons/System_ReturnToBlockland/server.cs") && !$RP::Crime::RTB)
{
	if (!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	$RP::Crime::RTB = true;
	
	// Server
	RTB_registerPref("Check Offline Criminals", "RP Content", "$RP::pref::server::checkOfflineCriminals", "bool", "RP Crime", $RP::pref::server::checkOfflineCriminals, false, true, "");
	RTB_registerPref("Disable Hammer Damage", "RP Content", "$RP::pref::server::disableDamageHammer", "bool", "RP Crime", $RP::pref::server::disableDamageHammer, false, true, "");

	// User
	RTB_registerPref("Display Commit Crime", "RP Content", "$RP::pref::user::displayCommitCrime", "bool", "RP Crime", $RP::pref::user::displayCommitCrime, false, true, "");
	RTB_registerPref("Time to return money", "RP Content", "$RP::pref::user::timeReturnMoney", "int 1 72", "RP Crime", $RP::pref::user::timeReturnMoney, false, true, "");

	// Demerits
	RTB_registerPref("Required Demerits to be Wanted", "RP Content", "$RP::pref::demerits::minimum", "int 100 5000", "RP Crime", $RP::pref::demerits::minimum, false, true, "");
	RTB_registerPref("Limit Blocking Access to Public Services", "RP Content", "$RP::pref::demerits::maximum", "int 500 100000", "RP Crime", $RP::pref::demerits::maximum, false, true, "");
	RTB_registerPref("Money to Pay For Clearing Records", "RP Content", "$RP::pref::demerits::clearRecords", "int 1000 100000", "RP Crime", $RP::pref::demerits::clearRecords, false, true, "");
	RTB_registerPref("Money to Pay For Each Time in Jail", "RP Content", "$RP::pref::demerits::pardonMultiplier", "int 500 100000", "RP Crime", $RP::pref::demerits::pardonMultiplier, false, true, "");
	RTB_registerPref("Demerits Reduced Each Hour", "RP Content", "$RP::pref::demerits::StatueOfLimitations", "int 0 500", "RP Crime", $RP::pref::demerits::StatueOfLimitations, false, true, "");
	RTB_registerPref("Jailtime Depending on Amount of Demerits", "RP Content", "$RP::pref::demerits::amountDemeritsTick", "int 100 1000", "RP Crime", $RP::pref::demerits::amountDemeritsTick, false, true, "");
	RTB_registerPref("Criminal Demerits Multiplier for Experience for the Jailer", "RP Content", "$RP::pref::demerits::jailExp", "int 100 1000", "RP Crime", $RP::pref::demerits::jailExp, false, true, "");

	// Crimes
	RTB_registerPref("Breaking And Entering", "RP Content", "$RP::pref::crime::BreakingAndEntering", "int 20 10000", "RP Crime", $RP::pref::crime::BreakingAndEntering, false, true, "");
	RTB_registerPref("Activate Alarm", "RP Content", "$RP::pref::crime::activateAlarm", "int 20 10000", "RP Crime", $RP::pref::crime::activateAlarm, false, true, "");
	RTB_registerPref("Pickpocket", "RP Content", "$RP::pref::crime::pickpocket", "int 20 10000", "RP Crime", $RP::pref::crime::pickpocket, false, true, "");
	RTB_registerPref("Bankrobbing", "RP Content", "$RP::pref::crime::bankrobbing", "int 20 10000", "RP Crime", $RP::pref::crime::bankrobbing, false, true, "");
	RTB_registerPref("Murderer", "RP Content", "$RP::pref::crime::murderer", "int 20 10000", "RP Crime", $RP::pref::crime::murderer, false, true, "");
	RTB_registerPref("Killing Innocents", "RP Content", "$RP::pref::crime::killInnocent", "int 20 10000", "RP Crime", $RP::pref::crime::killInnocent, false, true, "");
	RTB_registerPref("Illegally Claiming a Bounty", "RP Content", "$RP::pref::crime::illegalClaimBounty", "int 20 10000", "RP Crime", $RP::pref::crime::illegalClaimBounty, false, true, "");
	RTB_registerPref("Illegally Placing a Bounty", "RP Content", "$RP::pref::crime::placeBounty", "int 20 10000", "RP Crime", $RP::pref::crime::placeBounty, false, true, "");

	// Jail
	RTB_registerPref("Maximum Jailtime", "RP Content", "$RP::pref::jail::maxJailTime", "int 1 20", "RP Crime", $RP::pref::jail::maxJailTime, false, true, "");

	// Bounty
	RTB_registerPref("Amount of Money Before the Bounty is Hidden", "RP Content", "$RP::pref::bounty::hidePlaceBounty", "int 1 20", "RP Crime", $RP::pref::bounty::hidePlaceBounty, false, true, "");

	// Misc
	RTB_registerPref("Time Criminal Offset", "RP Content", "$RP::pref::timeCriminalOffset", "int 1 24", "RP Crime", $RP::pref::timeCriminalOffset, false, true, "");
	RTB_registerPref("Time Criminal Amount", "RP Content", "$RP::pref::timeCriminalAmount", "int 1 24", "RP Crime", $RP::pref::timeCriminalAmount, false, true, "");
}
