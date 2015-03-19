// --------------------------
// RP Crime :: User
// --------------------------

if (isPackage(RP_Crime_User))
	deactivatePackage(RP_Crime_User);

package RP_Crime_User
{
	// When the players gets killed
	function GameConnection::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc)
	{
		// The killer exists and is not yourself
		if (isObject(%killer) && %killer != %client)
		{
			// Immunity
			if (!$RP::job::immunity[%killer.RPData.value["jobID"]])
				CommitCrime(%killer, "Murder", $RP::pref::crime::murderer);
			// Person is not wanted
			else if (%client.RPData.value["demerits"] < $RP::pref::demerits::minimum)
				CommitCrime(%killer, "Killing innocents", $RP::pref::crime::killInnocent);
				
			%bountyMoney = %client.RPData.value["bounty"];
			// Have a bounty
			if (%bountyMoney > 0)
			{
				messageClient(%killer, '', "\c6You claimed \c3" @ %client.name @ "\c6's bounty for \c3$" @ %bountyMoney @ "\c6.");
				%killer.RPData.value["bank"] += %bountyMoney;
				messageClient(%killer, '', "\c6The money has been wired to your bank account.");
				// Bounty Hunter
				if ($RP::job::gainBounty[%killer.RPData.value["jobID"]])
					messageClient(%killer, '', "\c6You claimed a bounty.");
				// No Bounty Hunter
				else
					CommitCrime(%killer, "Claiming a bounty", $RP::pref::crime::illegalClaimBounty);
					
				RemoveFromBountyList(%client);
				%client.RPData.value["bounty"] = 0;
			}
		}
		Parent::onDeath(%client, %killerPlayer, %killer, %damageType, %damageLoc);
	}
	
	// Activating
	function Player::activateStuff(%player)
	{
		Parent::activateStuff(%player);
		
		// Retreive target
		%obj = %player.GetObjectFromPOW($TypeMasks::PlayerObjectType);
		%client = %player.client;
		if (!%client.CheckLastAction(%obj))
			return 0;
		
		if (!isObject(%obj))
			return 0;
		
		// Player
		if (%obj.getType() & $TypeMasks::PlayerObjectType)
		{
			if (!RPModExist("Jobs"))
				return 1;
			%target = %obj.client;
			if (!isObject(%target))
				return 1;
			%tKey = %target.getSaveKey();
			%special = $RP::job::special[%client.RPData.value["jobID"]];
			
			switch$ (firstWord(%special))
			{
				// Pickpocketing
				case "pickpocket":
					if (!RPModExist("Money"))
						return;
					// Cannot do this while in jail
					if (%client.RPData.value["jail"] > 0)
						return;
					if (getRandom(0, 1))
					{
						%available = RPDB.get(%tKey, "money");
						//%available = %target.RPData.value["money"];
						%max = getWord(%special, 1);
						%max = (%max > %available) ? %available : %max;
						if (%max <= 0)
							return 1;
						%amount = getRandom(1, %max);
						
						%percent = getWord(%special, 2);
						
						CommitCrime(%client, "Pickpocketing", $RP::pref::crime::pickpocket);
						
						if (%amount > 0)
						{
							RPDB.set(%tKey, "money", %available - %amount);
							%client.RPData.value["money"] += %amount;
							%client.RPData.value["exp"] += mfloor(%amount / 5);
							%client.RPData.value["CrimeExp"] += mfloor(%amount / 8);
							%client.RPData.value["stolenMoney"] += %amount;
							
							messageClient(%client, '', '\c6You have stolen \c3%1\c6 from \c3%2\c6.', prepareDisplayMoney(%amount), %target.name);
							messageClient(%target, '', '\c6Someone has stolen \c3%2\c6 from you.', %client.name, prepareDisplayMoney(%amount));
						}
						
						%client.displayInfo();
						%target.displayInfo();
					}
					%client.lastAction = $sim::time;
				
				// Demerits
				case "demerits":
					messageClient(%client, '', '\c3%1\c6 has \c3%2\c6 demerits.', %target.name, RPDB.get(%target.getSaveKey(), "demerits"));
					%client.lastAction = $sim::time;
			}
		}
		
		return 1;
	}
	
	// Player damage
	function Player::damage(%player, %attacker, %pos, %damage, %damageType)
	{
		%t = Parent::damage(%player, %attacker, %pos, %damage, %damageType);
		
		// Attacker exists and is not yourself
		if (isObject(%attacker) && %attacker != %player)
		{
			%target = %attacker.client;
			%client = %player.client;
			// Lethal damage
			if (%damage > 0)
			{
				
			}
		}
		
		return %t;
	}
	
	// Give default equipments for the job
	function Player::RP_giveDefaultEquipment(%player)
	{
		Parent::RP_giveDefaultEquipment(%player);
		
		%client = %player.client;
		if (!isObject(%client))
			return;
		
		if (%client.RPData.value["jail"] > 0 && !RPModExist("Jobs"))
		{
			%tools = "";
			%player.addTools(%tools);
			%client.RPData.value["tools"] = %tools;
		}
	}
	
	// Disable hammer damage
	function HammerImage::onHitObject(%image, %obj, %slot, %col, %pos, %normal)
	{
		%client = %col.client;
		if (%col.getType() & $TypeMasks::PlayerObjectType && isObject(%client))
			return;
		else
			Parent::onHitObject(%image, %obj, %slot, %col, %pos, %normal);
	}
	
	function serverCmdmessageSent(%client, %text)
	{
		if(%client.RPData.value["jail"] >= 1)
		{
			serverCmdteamMessageSent(%client, %text);
			return;
		}
		
		parent::serverCmdmessageSent(%client, %text);
	}
};
RPRegPackage(RP_Crime_User);

// Take back stolen money
function GameConnection::TakeDirtyMoney(%client, %target, %p)
{
	if (!(isObject(%client) && isObject(%target)))
		return false;

	%stolen = %target.RPData.value["stolenMoney"];
	// Have not stolen money
	if (%stolen == 0)
		return false;

	// Remove stolen money
	%target.RPData.value["bank"] += %target.RPData.value["money"];
	%target.RPData.value["money"] = 0;
	
	%target.RPData.value["bank"] -=  %target.RPData.value["stolenMoney"];

	if(%target.RPData.value["bank] < 0)
	{
		%target.RPData.value["jail"] += (%target.RPData.value["bank"] * -1) / 50;
	}
	// Give to police

	
	// Transfer stolen money
	%client.RPData.value["stolenMoney"] += %stolen;
	%client.RPData.value["money"] += %stolen;
	%target.RPData.value["stolenMoney"] = 0;
	
	messageClient(%target, '', '\c6The police took the stolen money from you.');
	messageClient(%client, '', '\c6You found \c3%1\c6 worth of stolen money.', prepareDisplayMoney(%stolen));
	messageClient(%client, '', '\c6You have \c3%1\c6 hours to return them to the nearest Police Department.', $RP::pref::user::timeReturnMoney);
	return true;
}

// Transfer dirty money
function GameConnection::TransferDirtyMoney(%client, %target, %amount)
{
	if (!(isObject(%client) && isObject(%target)))
		return;
	
	%stolen = %client.RPData.value["stolenMoney"];
	%amount = (%amount > %stolen) ? %stolen : %amount;
	%client.RPData.value["stolenMoney"] -= %amount;
	%target.RPData.value["stolenMoney"] += %amount;
}

//Hide Kills function Credit to Greek2Me
exec("./Support_SpecialKills.cs");

addSpecialDamageMsg(HideKill,"","");

function isSpecialKill_HideKill()
{
	return true;
}
