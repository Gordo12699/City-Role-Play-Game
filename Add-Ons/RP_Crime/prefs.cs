// --------------------------
// RP Crime :: Standard preferences
// --------------------------

// Server

$RP::pref::server::checkOfflineCriminals = 1;

// User

$RP::pref::user::displayCommitCrime = 1;
$RP::pref::user::timeReturnMoney = 24;

// Demerits

$RP::pref::demerits::minimum = 250; // The required demerits to be wanted
$RP::pref::demerits::maximum = 1000; // The limit to block access to public services
$RP::pref::demerits::clearRecords = 10000;
$RP::pref::demerits::pardonMultiplier = 4000;
$RP::pref::demerits::StatueOfLimitations = 30; // Amount of demerits removed each tick
$RP::pref::demerits::amountDemeritsTick = 200; // Amount of demerits for each tick when jailed
$RP::pref::demerits::jailExp = 100; // Demerits needed for each experience

// Crimes

$RP::pref::crime::BreakingAndEntering = 60;
$RP::pref::crime::activateAlarm = 30;
$RP::pref::crime::pickpocket = 25;
$RP::pref::crime::bankrobbing = 600;
$RP::pref::crime::murderer = 200;
$RP::pref::crime::killInnocent = 100;
$RP::pref::crime::illegalClaimBounty = 400;
$RP::pref::crime::placeBounty = 200; // Illegally placing a bounty

// Jail

$RP::pref::jail::maxJailTime = 60;
$RP::pref::jail::job = "Inmate";

// Bounty

$RP::pref::bounty::hidePlaceBounty = 500; // Amount of money before the bounty is hidden

// Misc

$RP::pref::timeCriminalOffset = 23;
$RP::pref::timeCriminalAmount = 4;

$RP::pref::crime::havingDrugs = 50;
