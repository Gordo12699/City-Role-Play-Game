// --------------------------
// RP Money :: System
// --------------------------

// Preparing displaying money
function prepareDisplayMoney(%amount)
{
	return $RP::pref::moneyPrefix @ %amount @ $RP::pref::moneySuffix;
}
