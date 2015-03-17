// ============================================================
// Project				:	CRPG
// Author				:	Iban
// Description			:	Server Preferences
// ============================================================
// Table of Contents
// 1. Basic Preferences
// 2. Price Preferences
// 2.1 Weapon Price Preferences
// 3. Tick Preferences
// 4. Demerit Preferences
// 5. Vehicle Preferences
// 7. Allowed Inmate Items
// 8. Food
// 9. Banned Events
// 10. Hunger Colors and States
// ============================================================

// ============================================================
// Section 3.5 : Drug Preferences
// ============================================================
// Misc Drug Prefs
$CRPG::drug::minSellSpeed = 1; // In seconds
$CRPG::drug::maxSellSpeed = 10; // In seconds
$CRPG::drug::minBuyAmt = 1; // Minimum grams of weed player is capable of selling // Grams 1 - 5 have special names
$CRPG::drug::maxBuyAmt = 5; // Maximum ^
$CRPG::drug::sellPrice = 10; // About the real value of a gram of weed in the US // The actual price randomly changes by a couple digits
$CRPG::drug::maxdrugplants = 6;
$CRPG::drug::sellTimes = 50;
$CRPG::drug::demWorth = 3; // The amount of dems each gram is worth. If their grams are worth the wanted limit or higher, they can be jailed.

// Drug Color Prefs
$CRPG::drug::startcolor = 45;
$CRPG::drug::emittertype = GrassEmitter;

// Drug Evidence Prefs
$CRPG::drug::evidenceWorth = 500; // How much someone can turn in drug evidence for

// Drug Types
// -Marijuana
$CRPG::drugs::marijuana::placePrice = 1800; // How much it costs to plant the brick
$CRPG::drugs::marijuana::harvestMin = 9; // Amount of grams you get from harvest
$CRPG::drugs::marijuana::harvestMax = 14;
$CRPG::drugs::marijuana::growthTime = 8; // In minutes
$CRPG::drugs::marijuana::basePrice = getRandom(9,11); // Price per gram
// -Opium
$CRPG::drugs::opium::placePrice = 3000;
$CRPG::drugs::opium::harvestMin = 11;
$CRPG::drugs::opium::harvestMax = 18;
$CRPG::drugs::opium::growthTime = 8;
$CRPG::drugs::opium::basePrice = getRandom(12,16);
