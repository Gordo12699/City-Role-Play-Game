// --------------------------
// RP Money :: Init Load
// Info: This is where everything
// that have to be loaded directly goes
// --------------------------

// Load bricks
exec("./bricks/bank.cs");
exec("./bricks/atm.cs");
exec("./bricks/safe.cs");

// Input
registerInputEvent(fxDTSBrick, onTransferSuccess, "Self fxDTSBrick\tPlayer Player\tClient GameConnection");

// Output
//registerOutputEvent(fxDTSBrick, transferMoneyToBrick, "string 30 100" TAB "int 0 10000 0", 1);
//registerOutputEvent(fxDTSBrick, transferMoneyToClient, "list Closed 4 Owner 3 Trust2 2 Trust1 1 Anyone 0", 1);
//registerOutputEvent(fxDTSBrick, displayCurrentMoney, "", 1);

function CRPG_AssembleEvents()
{	
	for(%c = 1; %c <= $ListAmt; %c++)
	{
		%sellItem_List = %sellItem_List SPC strreplace($CRPG::prices::weapon::name[%c].uiName, " ", "") SPC %c;
	}
	registerOutputEvent("fxDTSBrick", "RPsellItem", "list" @ %sellItem_List TAB "int 0 500 1");
}
// Standard
exec("./prefs.cs");

// Reload preferences
LoadPreferences();

// Create RTB Preferences
if (isFile("Add-Ons/System_ReturnToBlockland/server.cs") && !$RP::Money::RTB)
{
	if (!$RTB::RTBR_ServerControl_Hook)
	{
		exec("Add-Ons/System_ReturnToBlockland/RTBR_ServerControl_Hook.cs");
	}
	$RP::Money::RTB = true;
	
	// Server
	RTB_registerPref("Brick Cost", "RP Content", "$RP::pref::server::brickCost", "int 0 10", "RP Money", $RP::pref::server::brickCost, false, true, "");

	// User
	RTB_registerPref("Start Money", "RP Content", "$RP::pref::user::startMoney", "int 0 1000", "RP Money", $RP::pref::user::startMoney, false, true, "");
	RTB_registerPref("Each Hour Money", "RP Content", "$RP::pref::user::tickMoney", "int 0 200", "RP Money", $RP::pref::user::tickMoney, false, true, "");
	
	// Misc
	RTB_registerPref("Name of Money", "RP Content", "$RP::pref::moneyName", "string 16", "RP Money", $RP::pref::moneyName, false, true, "");
	RTB_registerPref("Money Prefix", "RP Content", "$RP::pref::moneyPrefix", "string 4", "RP Money", $RP::pref::moneyPrefix, false, true, "");
	RTB_registerPref("Money Suffix", "RP Content", "$RP::pref::moneySuffix", "string 4", "RP Money", $RP::pref::moneySuffix, false, true, "");
	RTB_registerPref("Maximum Loan", "RP Content", "$RP::pref::loan::maximum", "int 0 10000", "RP Money", $RP::pref::loan::maximum, false, true, "");
	RTB_registerPref("Loan Payback Each Day", "RP Content", "$RP::pref::loan::payback", "int 0 100", "RP Money", $RP::pref::loan::payback, false, true, "");
	RTB_registerPref("Interest Bank", "RP Content", "$RP::pref::interest::bank", "int 0 1000", "RP Money", $RP::pref::interest::bank, false, true, "");
	RTB_registerPref("Interest Loan", "RP Content", "$RP::pref::interest::loan", "int 0 1000", "RP Money", $RP::pref::interest::loan, false, true, "");
	RTB_registerPref("Interest Safe", "RP Content", "$RP::pref::interest::safe", "int 0 1000", "RP Money", $RP::pref::interest::safe, false, true, "");
}
