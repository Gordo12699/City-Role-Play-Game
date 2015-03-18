// --------------------------
// RP Money :: Game
// --------------------------

if (isPackage(RP_Money_Game_Boot))
	deactivatePackage(RP_Money_Game_Boot);

package RP_Money_Game_Boot
{
	// Booting game
	function BootRP()
	{
		if ($RP::activated)
			return;
		Parent::BootRP();
		
		RPDB.addValue("money", $RP::pref::user::startMoney);
		RPDB.addValue("bank", 0);
		RPDB.addValue("loan", 0);
		RPDB.addValue("safe", 0);
		RPDB.addValue("lotData", 0);
		RPDB.addBool("tickBank", 0);
		RPDB.saveDefs();
		
		RP_AddStat($RP::pref::moneyName, "money", 1, "", $RP::pref::moneyPrefix, $RP::pref::moneySuffix);
		RP_AddStat("Bank", "bank", 0, "", $RP::pref::moneyPrefix, $RP::pref::moneySuffix);
		RP_AddStat("Loan", "loan", 0, "", $RP::pref::moneyPrefix, $RP::pref::moneySuffix);
		RP_AddStat("Safe", "safe", 0, "", $RP::pref::moneyPrefix, $RP::pref::moneySuffix);
		RP_AddStat("LotData", "lotData", 0);
		
		CRPG_AssembleEvents();
	}
};
activatePackage(RP_Money_Game_Boot);

if (isPackage(RP_Money_Game))
	deactivatePackage(RP_Money_Game);

package RP_Money_Game
{	
	// RP Time
	function CheckTickID(%ID)
	{
		%name = RPDB.get(%ID, "name");
		%client = findClientByName(%name);
		if (!isObject(%client))
			return false;
		// Player has not respawned yet
		if (!isObject(%client.player))
		{
			messageClient(%client, '', "\c6 - You received nothing because you have not spawned yet.");
			return false;
		}
		
		if (!RPModExist("Jobs"))
		{
			// Transfer to bank
			if (%client.RPData.getBool("tickBank"))
			{
				%client.RPData.value["bank"] += $RP::pref::user::tickMoney;
				messageClient(%client, '', '\c6 - \c3%1\c6 has been paid on your bank account.', prepareDisplayMoney($RP::pref::user::tickMoney));
			}
			// Transfer to player
			else
			{
				%client.RPData.value["money"] += $RP::pref::user::tickMoney;
				messageClient(%client, '', '\c6 - \c6You received \c3%1\c6 from bank.', prepareDisplayMoney($RP::pref::user::tickMoney));
			}
			%client.displayInfo();
		}
		
		// New day, new taxround
		if ($RP::pref::timeHour == 12)
		{
			CheckTaxID(%ID);
		}
		
		$RP::pref::economy = 0;
		for(%i = 0;%i < ClientGroup.getCount();%i++) 
		{
			%client = ClientGroup.getObject(%i);
			$RP::pref::economy += %client.RPData.value["money"] + %client.RPData.value["bank"];
		}

		Parent::CheckTickID(%ID);
	}
};
RPRegPackage(RP_Money_Game);

// Check player tax and investement
function CheckTaxID(%ID)
{
	%name = RPDB.get(%ID, "name");
	%client = findClientByName(%name);
	if (!isObject(%client))
		return false;
	// In jail means no taxes
	if (RPModExist("Crime") && %client.RPData.value["jail"] > 0)
		return false;
	
	%bank = %client.RPData.value["bank"];
	%loan = %client.RPData.value["loan"];
	%safe = %client.RPData.value["safe"];
	
	// Bank interest
	if ($RP::pref::interest::bank != 0 && %bank > 0)
	{
		%percent = mFloor(%bank * ($RP::pref::interest::bank / 1000));
		if (%percent > 0)
		{
			%client.RPData.value["bank"] += %percent;
			messageClient(%client, '', '\c6 - You got interest \c3%1\c6 on your bank.', prepareDisplayMoney(%percent));
		}
	}
	
	// Loan interest
	if ($RP::pref::interest::loan != 0 && %loan > 0)
	{
		%percent = %loan * ($RP::pref::interest::loan / 1000);
		%client.RPData.value["loan"] += mFloor(%percent);
		%client.RPData.value["loan"] -= $RP::pref::loan::payback;
		%client.RPData.value["money"] -= $RP::pref::loan::payback;
		messageClient(%client, '', '\c6 - You paid \c3%1\c6 of your loan.', prepareDisplayMoney($RP::pref::loan::payback));
	}
	
	%client.displayInfo();
	return true;
}

// Request to transfer money
function RequestTransfer(%client, %brick, %type, %amount, %answer)
{
	if (!(isObject(%client) && isObject(%brick)))
		return false;
	
	%brickGroup = getBrickGroupFromObject(%brick);
	%target = %brickGroup.client;
	
	// Not yourself
	if (%client == %target)
	{
		messageClient(%client, '', "\c6Services are for customers only.");
		return false;
	}
	
	// Somehow get the name
	%name = (!isObject(%target)) ? %brickGroup.name : RPDB.get(%target.getSaveKey(), "name");
	
	if (%client.activateState == 1)
	{
		%client.activateState = 0;
		// Decline
		if (strLwr(%answer) !$= "yes")
		{
			messageClient(%client, '', "\c6You declined the offer.");
			return false;
		}
		
		// Transfer credits
		%client.RPData.value["money"] -= %amount;
		%brick.RP_money += %amount;
		
		messageClient(%client, '', '\c6You bought %1 \c3%2\c6 from \c3%3\c6 for \c3%4\c6.', findVowel(%type), %type, %name, prepareDisplayMoney(%amount));
		
		if (isObject(%target))
			messageClient(%target, '', '\c3%1\c6 has bought %2 \c3%3\c6 from you for \c3%4\c6.', %client.name, findVowel(%type), %type, prepareDisplayMoney(%amount));
		
		%client.StopActivate();
		if (isEventPending(%client.activateSchedule2))
			Cancel(%client.activateSchedule2);
		%brick.onTransferSuccess(%client);
	}
	else
	{
		// Not enough money
		if (%client.RPData.value["money"] < %amount)
		{
			messageClient(%client, '', "\c6You cannot afford this.");
			return false;
		}
		messageClient(%client, '', '\c6You want to buy %1 \c3%2\c6 from \c3%3\c6 for \c3%4\c6.', findVowel(%type), %type, %name, prepareDisplayMoney(%amount));
		messageClient(%client, '', "\c6Type \c3yes\c6 to accept.");
		%client.activateState = 1;
		
		if (isEventPending(%client.activateSchedule2))
			Cancel(%client.activateSchedule2);
		%client.activateSchedule2 = schedule(1000 * $RP::pref::user::stopActivate, 0, RequestTransfer, %client, %type, %amount, "no");
		%client.ThrowLastActive();
	}
	return true;
}

// Buy an item
function BuyItem(%client, %brick, %datablock)
{
	if (!(isObject(%client) && isObject(%brick) && isObject(%datablock)))
		return false;
	
	%money = atoi(%brick.RP_money);
	%damage = mAbs(%datablock.image.projectile.directDamage);
	%cost = $RP::pref::item::cost + (%damage * $RP::pref::item::multiplier);
	%name = %datablock.uiName;
	%money -= %cost;
	
	if (%money < 0)
	{
		messageClient(%client, '', '\c6The brick need \c3%1\c6 to buy %2 \c3%3\c6.', prepareDisplayMoney(mAbs(%money)), findVowel(%name), %name);
		return false;
	}
	%brick.RP_money = %money - %cost;
	
	messageClient(%client, '', '\c6You bought %1 \c3%2\c6 for \c3%3\c6.', findVowel(%name), %name, prepareDisplayMoney(%cost));
	return true;
}
