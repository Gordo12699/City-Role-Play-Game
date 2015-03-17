// Job Brick

// Brick Data
datablock fxDtsBrickData(JobBrickData)
{
	brickfile = "./TvPurple.blb";
	iconName = "Add-Ons/RP_Core/LcdTV";		
	
	category = "CRPG";
	subcategory = "Admin event bricks";
	
	uiName = "Employment Brick";
	
	specialBrickType = "";
	
	isInfoBrick = 1;
	adminOnly = 1;
};

// Brick functions

// Activate Data
function JobBrickData::Activate(%datablock, %client, %obj)
{
	messageClient(%client, '', "\c3Employment Office");
	messageClient(%client, '', "\c31. \c6Job list");
	messageClient(%client, '', "\c32. \c6Specify job");
	messageClient(%client, '', "\c33. \c6Get job");
	messageClient(%client, '', "\c34. \c6Apply for Shop-Keeping License");
	%client.activateDatablock = %datablock;
	%client.activateState = 0;
	return true;
}

// Handle Data
function JobBrickData::parseData(%datablock, %client, %text)
{
	%client.ThrowLastActive();
	%number = atoi(%text);
	switch (%client.activateState)
	{
		case 0:
			switch (%number)
			{
				// Job list
				case 1:
					%client.StopActivate();
					DisplayJobsExtra(%client);
				// Check job
				case 2:
					DisplayJobsSimple(%client);
					%client.activateState = 2.1;
				// Get job
				case 3:
					DisplayJobsSimple(%client);
					%client.activateState = 3.1;
				case 4:
					if(%client.RPData.value["shopLicense"] == 1)
					{
						%client.StopActivate();
						messageClient(%client, '', "\c6You already have a license.");
					}
					
					messageClient(%client, '', "\c6The \c3Shop-Keeping License \c6allows players to sell items while maintaining a job different than Shop Keeper or Employee.");
					messageClient(%client, '', "\c6It costs \c3$" @ $RP::pref::server::ShopLicenseCost @ " \c6to purchase this license.");
					messageClient(%client, '', "\c6To apply for this license, say \c31 \c6in the chat.");
					%client.activateState = 4.1;
					
				// Invalid
				default:
					%client.StopActivate();
					return false;
			}
		// Check detailed job
		case 2.1:
			%client.StopActivate();
			if (%text $= "")
				return false;
			
			if (!DisplayJobDetail(%client, %text))
				messageClient(%client, '', '\c6Job [\c3%1\c6] does not exist.', %text);
		// Get job
		case 3.1:
			%client.StopActivate();
			
			if (%text $= "")
				return false;
			
			// Go throught jobs
			%job = FindJobIDByName(%text);
			if (%job == 0)
			{
				messageClient(%client, '', '\c6Job [\c3%1\c6] does not exist.', %text);
				return true;
			}
			
			// Check if the job is hidden
			if ($RP::job::hidden[%job])
			{
				messageClient(%client, '', '\c6Job [\c3%1\c6] is acquired through a promotion.', %text);
				return true;
			}
			
			// Check if player have money to pay for the job
			if (%client.RPData.value["exp"] < $RP::job::experience[%job])
			{
				messageClient(%client, '', "\c6Not enough experience.");
				return true;
			}
			
			// Check if player have money to pay for the job
			if (%client.RPData.value["education"] < $RP::job::education[%job])
			{
				messageClient(%client, '', "\c6You are not educated enough to get this job.");
				return true;
			}
			
			// Crime :: Check if have a clean record
			if (RPModExist("Crime") && $RP::job::clean[%job] && %client.RPData.value["jail"] >= 0)
			{
				messageClient(%client, '', "\c6You do not have a clean record to have that job.");
				return true;
			}
			
			%group = $RP::job::group[%client.RPData.value["jobID"]];
			if(%group $= "FFFFFF")
			{
				%client.RPData.value["exp"] -= 100;
				if(%client.RPData.value["exp"] < 0)
				{
					%client.RPData.value["exp"] = 0;
				}
			}
			else if(%group $= "999999")
			{
				%client.RPData.value["LaborExp"] -= 100;
				if(%client.RPData.value["LaborExp"] < 0)
				{
					%client.RPData.value["LaborExp"] = 0;
				}
			}
			else if(%group $= "333333")
			{
				%client.RPData.value["CrimeExp"] -= 100;
				if(%client.RPData.value["CrimeExp"] < 0)
				{
					%client.RPData.value["CrimeExp"] = 0;
				}
			}
			else if(%group $= "CCCC00")
			{
				%client.RPData.value["LawExp"] -= 100;
				if(%client.RPData.value["LawExp"] < 0)
				{
					%client.RPData.value["LawExp"] = 0;
				}
			}
			else if(%group $= "0000CC")
			{
				%client.RPData.value["BusinessExp"] -= 100;
				if(%client.RPData.value["BusinessExp"] < 0)
				{
					%client.RPData.value["BusinessExp"] = 0;
				}
			}
			else if(%group $= "CC00CC")
			{
				%client.RPData.value["MedicalExpExp"] -= 100;
				if(%client.RPData.value["MedicalExpExp"] < 0)
				{
					%client.RPData.value["MedicalExp"] = 0;
				}
			}
			else
			{
				if(%client.RPData.value["exp"] - 100 <= 0)
				{
					%client.RPData.value["exp"]= 0;
				}
				else
				{
					%client.RPData.value["exp"] -= 100;
				}
			}
			%client.RPData.value["jobID"] = %job;
			
			messageClient(%client, '', '\c6You are now %1 \c3%2\c6.', findVowel($RP::job::name[%job]), $RP::job::name[%job]);
			%client.applyForcedBodyColors();
			%client.applyForcedBodyParts();
			// Reset equipment
			%client.RPData.value["tools"] = "";
			// Reset player
			%client.FinishRPSpawning(true);
			
			return true;
			
		case 4.1:
			%client.StopActivate();
			if (%text $= "1")
			{
				if(%client.RPData.value["money"] >= $RP::pref::server::ShopLicenseCost)
				{
					%client.RPData.value["money"] -= $RP::pref::server::ShopLicenseCost;
					%client.RPData.value["shopLicense"] = 1;
					messageClient(%client, '', "\c6Congratulations! You have acquired a \c3Shop-Keeping License\c6!");
					
					return true;
				}
				else
				{
					messageClient(%client, '', "\c6You do not have enough money to purchase the \c3Shop-Keeping License\c6.");
					
					%client.StopActivate();
					return false;
				}
			}
			else
			{
				%client.StopActivate();
				return false;
			}
			
		// Invalid
		default:
			%client.StopActivate();
			return false;
	}
	return true;
}

// Displays the jobs in row
function DisplayJobsSimple(%client)
{
	messageClient(%client, '', "\c6Type one of the jobs below.");
	%text = "";
	%n = 0;
	for (%i = 1; %i <= $RP::jobs; %i++)
	{
		%ID = $RP::job::listID[%i];
		// Job is hidden
		//if ($RP::job::hidden[%ID])
		//	continue;
		// Job requires an expension to work
		if (!RPModsExists($RP::job::requireMod[%ID]))
			continue;
		
		%n++;
		%text = %text @ "\c3" @ $RP::job::name[%ID];
		// More jobs
		%text = %text @ "\c6, ";
		// Next row
		if (%n == 5)
		{
			messageClient(%client, '', %text);
			%text = "";
			%n = 0;
		}
	}
	// Display the last row
	if (%n > 0)
		messageClient(%client, '', %text);
}

// Displays more information from a job list
function DisplayJobsExtra(%client)
{
	messageClient(%client, '', "\c3Job list");
	for (%i = 1; %i <= $RP::jobs; %i++)
	{
		%ID = $RP::job::listID[%i];
		// Job requires an expension to work
		if (!RPModsExists($RP::job::requireMod[%ID]))
			continue;
		
		// Name and experience
		%text = "\c6Name:\c3 " @ $RP::job::name[%ID] @ " \c6Exp:\c3 " @ $RP::job::experience[%ID] @ " \c6Career Experience:\c3 " @ $RP::job::careerExperience[%ID];
		if (RPModsExists("Money Time"))
		{
			%times = getWordCount($RP::job::payTime[%ID]);
			// Payment
			switch (%times)
			{
				// No pay
				case 0:
					%text = %text @ " \c6Pay: \c3N/A \c6Paytime: \c3N/A";
				// From time to time
				case 2:
					%text = %text @ " \c6Pay:\c3 " @ $RP::job::pay[%ID] 
					@ " \c6Paytime:\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) 
					@ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true);
				// Hourly job
				default:
					%text = %text @ " \c6Pay:\c3 " @ $RP::job::pay[%ID] @ " \c6Paytime:\c3";
					for (%n = 0; %n < %times; %n++)
					{
						%text = %text SPC getClockTime(getWord($RP::job::payTime[%ID], %n), $RP::pref::game::am_pm, true);
						if (%n < %times - 1)
							%text = %text @ "\c6,\c3";
					}
			}
		}
		messageClient(%client, '', %text);
	}
}

// Display the job in detail
function DisplayJobDetail(%client, %job)
{
	%ID = FindJobIDByName(%job);
	if (%ID == 0)
		return false;
	
	if (!RPModsExists($RP::job::requireMod[%ID]))
		return false;
	
	messageClient(%client, '', "\c3" @ $RP::job::name[%ID]);
	messageClient(%client, '', "\c6Experience:\c3 " @ $RP::job::experience[%ID]);
	messageClient(%client, '', "\c6Career Experience:\c3 " @ $RP::job::careerExperience[%ID]);
	if (RPModsExists("Money Time"))
	{
		%text = "";
		// Payment
		switch (getWordCount($RP::job::payTime[%ID]))
		{
			// No pay
			case 0:
				%text = %text @ "\c6Pay: \c3N/A \c6Paytime: \c3N/A";
			// From time to time
			case 2:
				%text = %text @ "\c6Pay:\c3 " @ $RP::job::pay[%ID] @ " \c6Paytime:\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 0), $RP::pref::game::am_pm, true) @ " \c6to\c3 " @ getClockTime(getWord($RP::job::payTime[%ID], 1), $RP::pref::game::am_pm, true);
			// Hourly job
			default:
				%text = %text @ "\c6Pay:\c3 " @ $RP::job::pay[%ID] @ " \c6Paytime:\c3";
				%times = getWordCount($RP::job::payTime[%ID]);
				for (%n = 0; %n < %times; %n++)
				{
					%text = %text SPC getClockTime(getWord($RP::job::payTime[%ID], %n), $RP::pref::game::am_pm, true);
					if (%n < %times - 1)
						%text = %text @ "\c6,\c3";
				}
		}
		messageClient(%client, '', %text);
	}
	// Promotions
	%promotions = $RP::job::promotion[%ID];
	if (%promotions !$= "")
	{
		%n = getWordCount(%promotions);
		%promo = "";
		for (%i = 0; %i < %n; %i++)
		{
			%t = atoi(getWord(%promotions, %i));
			if (%t == 0)
				continue;
			%name = $RP::job::name[%t];
			if (RPModsExists($RP::job::requireMod[%t]))
				%promo = (%promo $= "") ? %name : %promo @ " or " @ %name;
		}
		// Display promotion
		if (%promo !$= "")
			messageClient(%client, '', "\c6Promotions: \c3" @ %promo);
	}
	// Display promotion if there is one and the required mod is correct
	//if (%promotion !$= "" && RPExistMods($RP::job::requireMod[%promotion]))
	//	messageClient(%client, '', "\c6Promotions: \c3" @ $RP::job::name[%promotion]);
	// Display special information about the job
	if ($RP::job::info[%ID] !$= "")
		messageClient(%client, '', "\c6" @ $RP::job::info[%ID]);
	
	return true;
}
