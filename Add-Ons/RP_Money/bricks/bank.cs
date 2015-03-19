// Bank Brick

// Brick Data
datablock fxDtsBrickData(BankBrickData)
{
	brickfile = "./TvGreen.blb";
	iconName = "Add-Ons/RP_Core/LcdTV";	
	
	category = "CRPG";
	subcategory = "Admin event bricks";
	
	uiName = "Bank Brick";
	
	specialBrickType = "";
	
	guiCmd = "Bank";
	mod = "Money";
	
	isInfoBrick = 1;
	adminOnly = 1;
};

// Brick functions

// Activate data
function BankBrickData::Activate(%datablock, %client)
{
	messageClient(%client, '', "\c3Bank");
	
	%loan = %client.RPData.value["loan"];
	
	messageClient(%client, '', '\c6You have \c3%1\c6 in your bank account.', prepareDisplayMoney(getMetricPrefix(%client.RPData.value["bank"])));
	// You have loaned
	if (%loan > 0)
		messageClient(%client, '', '\c6You have \c3%1\c6 loan left to pay.', prepareDisplayMoney(%loan));
	
	messageClient(%client, '', '\c31. \c6Withdraw %1', $RP::pref::moneyName);
	messageClient(%client, '', '\c32. \c6Deposit %1', $RP::pref::moneyName);
	messageClient(%client, '', '\c33. \c6Deposit all %1', $RP::pref::moneyName);
	messageClient(%client, '', '\c34. \c6Transfer %1', $RP::pref::moneyName);
	
	// You have reached maximum in loan
	if (%loan < $RP::pref::loan::maximum)
		messageClient(%client, '', "\c35. \c6Loan credits");
	// You have loans
	if (%loan > 0)
	{
		messageClient(%client, '', "\c36. \c6Pay back loan");
		messageClient(%client, '', "\c37. \c6Pay back the whole loan");
	}
	// Bankrobbers
	if (RPModExist("Crime") && firstWord($RP::job::special[%client.RPData.value["jobID"]]) $= "bankrobbing")
		messageClient(%client, '', "\c38. \c6Rob the bank");
	
	if (RPModExist("Time"))
	{
		%where = (%client.RPData.getBool("tickBank")) ? "Bank" : "Yourself";
		messageClient(%client, '', '\c39. \c6Change where tick money %1 will be received. Currently in [\c3%2\c6].', $RP::pref::moneyName, %where);
	}
	
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

// Handle data
function BankBrickData::parseData(%datablock, %client, %text)
{	
	%client.ThrowLastActive();
	if (%client.activateStage != 4.1)
		%number = atoi(%text);
	switch (%client.activateState)
	{
		// Start
		case 0:
			switch (%number)
			{
				// Withdraw
				case 1:
					messageClient(%client, '', '\c6Insert the amount of %1 to be withdrawn.', $RP::pref::moneyName);
					%client.activateState = 1.1;
				// Deposit
				case 2:
					messageClient(%client, '', '\c6Insert the amount of %1 to be deposited.', $RP::pref::moneyName);
					%client.activateState = 2.1;
				// Deposit all
				case 3:
					messageClient(%client, '', '\c6You have deposited \c3%1\c6.', prepareDisplayMoney(%client.RPData.value["money"]));
					%client.RPData.value["bank"] += %client.RPData.value["money"];
					%client.RPData.value["money"] = 0;
					%client.stopActivate();
				// Transfer
				case 4:
					messageClient(%client, '', "\c6Insert the \c3name \c6or \c3BL_ID \c6of the receiver.");
					%client.activateState = 4.1;
				// Loan
				case 5:
					if (%client.RPData.value["loan"] >= $RP::pref::loan::maximum)
					{
						%client.stopActivate();
						return false;
					}
					
					messageClient(%client, '', "\c6Insert the amount of credits you want to loan.");
					%client.activateState = 5.1;
				// Pay back
				case 6:
					if (%client.RPData.value["loan"] == 0)
					{
						%client.stopActivate();
						return false;
					}
					
					messageClient(%client, '', "\c6Insert the amount of credits you want to pay back. (1 - 2000)");
					%client.activateState = 6.1;
				// Pay back all
				case 7:
					%client.stopActivate();
					if (%client.RPData.value["loan"] == 0)
						return false;
					
					%pay = (%client.RPData.value["money"] >= %client.RPData.value["loan"]) ? %client.RPData.value["loan"] : %client.RPData.value["money"];
					%client.RPData.value["loan"] -= %pay;
					%client.RPData.value["money"] -= %pay;
					
					messageClient(%client, '', '\c6You have paid back \c3%1\c6 and have \c3%2\c6 left.', prepareDisplayMoney(%pay), prepareDisplayMoney(%client.RPData.value["loan"]));
					%client.displayInfo();
				// Bank rob
				case 8:
					%client.stopActivate();
					
					//Check if brick is still there
					if(!isObject(%datablock))
						return false;
					// Requires the Crime extension
					if (!RPModExist("Crime"))
						return false;
					
					// Can actually rob a bank at all
					if (firstWord($RP::job::special[%client.RPData.value["jobID"]]) !$= "bankrobbing")
						return false;
					
					%max = mFloor(getWord($RP::job::special[%client.RPData.value["jobID"]], 1));
					%amount = getRandom(mFloor(%max / 2), %max);
					%client.RPData.value["money"] += %amount; // Add money to player
					%client.RPData.value["stolenMoney"] += %amount; // Add stolen money to player
					%client.RPData.value["exp"] += mFloor(%amount / 10); // Give the player experience
					CommitCrime(%client, "Bankrobbing", $RP::pref::crime::bankrobbing);
					messageAll('', "\c3The bank has been robbed!");
					%client.displayInfo();
				// Change recieving
				case 9:
					%client.stopActivate();
					
					// Requires the Time extension
					if (!RPModExist("Time"))
						return false;
					
					%bool = !%client.RPData.getBool("tickBank");
					%client.RPData.setBool("tickBank", %bool);
					%where = (%bool) ? "Bank" : "Yourself";
					messageClient(%client, '', '\c6Tick money %1 will now be received to [\c3%2\c6].', $RP::pref::moneyName, %where);
				// Invalid
				default:
					%client.stopActivate();
					return 0;
			}
		// Withdraw money
		case 1.1:
			%client.stopActivate();
			if (%number <= 0)
				return 0;
			
			%number = (%number > %client.RPData.value["bank"]) ? %client.RPData.value["bank"] : %number;
			messageClient(%client, '', '\c6You have withdrawn \c3%1\c6.', prepareDisplayMoney(getMetricPrefix(%number)));
			%client.RPData.value["bank"] -= %number;
			%client.RPData.value["money"] += %number;
			messageClient(%client, '', '\c6You now have \c3%1\c6 in your bank account.', prepareDisplayMoney(getMetricPrefix(%client.RPData.value["bank"])));
			
		// Deposit money
		case 2.1:
			%client.stopActivate();
			if (%number <= 0)
				return 0;
			
			%number = (%number > %client.RPData.value["money"]) ? %client.RPData.value["money"] : %number;
			messageClient(%client, '', '\c6You have deposited \c3%1\c6.', prepareDisplayMoney(getMetricPrefix(%number)));
			%client.RPData.value["bank"] += %number;
			%client.RPData.value["money"] -= %number;
		// Transfer money
		case 4.1:
			%target = findClientByName(%text);
			if (!isObject(%target))
				%target = findClientByBL_ID(%text);
			if (!isObject(%target))
			{
				%client.stopActivate();
				return 0;
			}
			
			// Cannot transfer to himself
			if (%target == %client)
			{
				%client.stopActivate();
				messageClient(%client, '', "\c6You cannot transfer money to yourself.");
				return 1;
			}
			messageClient(%client, '', '\c6Insert the amount of %1 to be transferred.', $RP::pref::moneyName);
			%client.activateState = 4.2;
			%client.RPtarget = %target;
		// Transfer money
		case 4.2:
			%client.stopActivate();
			%target = %client.RPtarget;
			%client.RPtarget = "";
			%number = %text;
			if (%number <= 0)
				return 0;
			if (!isObject(%target))
			{
				messageClient(%client, '', "\c6Target was lost.");
				return 1;
			}
			
			// Reached highest amount
			if(%number > %client.RPData.value["bank"])
			{
				%number = %client.RPData.value["bank"];
			}
			else
			{
				%number = %number;
			}
			
			messageClient(%client, '', '\c6You have transferred \c3%1\c6 to \c3%2\c6.', prepareDisplayMoney(getMetricPrefix(%number)), %target.name);
			messageClient(%target, '', '\c6You have received \c3%1\c6 from \c3%2 \c6to your bank account.', prepareDisplayMoney(getMetricPrefix(%number)), %client.name);
			
			%client.RPData.value["bank"] -= %number;
			%target.RPData.value["bank"] += %number;
		// Loan
		case 5.1:
			%client.stopActivate();
			if (%number > 0)
			{
				%number = (%number > $RP::pref::loan::maximum) ? $RP::pref::loan::maximum : %number;
				messageClient(%client, '', '\c6You loaned \c3%1\c6.', prepareDisplayMoney(getMetricPrefix(%number)));
				%client.RPData.value["loan"] += %number;
				%client.RPData.value["money"] += %number;
				%client.displayInfo();
			}
			else
				return false;
		// Pay back
		case 6.1:
			%client.stopActivate();
			if (%number > 0)
			{
				%number = (%number > %client.RPData.value["loan"]) ? %client.RPData.value["loan"] : %number;
				%number = (%number > %client.RPData.value["money"]) ? %client.RPData.value["money"] : %number;
				%client.RPData.value["loan"] -= %number;
				%client.RPData.value["money"] -= %number;
				messageClient(%client, '', '\c6You paid back \c3%1\c6 of your loan. You have \c3%2\c6 of the loan left.', prepareDisplayMoney(getMetricPrefix(%number)), prepareDisplayMoney(%client.RPData.value["loan"]));
				%client.displayInfo();
			}
			else
				return false;
		// Invalid
		default:
			%client.stopActivate();
			return 0;
	}
	return 1;
}

// GUI
function BankBrickData::ConstructGUI(%datablock, %client)
{
	commandToClient(%client, 'Money_Bank_SetData', "money " @ %client.RPData.value["money"] TAB 
		"bank " @ %client.RPData.value["bank"] TAB 
		"loan " @ %client.RPData.value["loan"]);
	%client.activateDatablock = %datablock;
}
