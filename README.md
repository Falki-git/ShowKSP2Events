# ShowKSP2Events
Shows events triggered by Kerbal Space Program 2.

This is a mod to help modders mod.

<span style="color: white;">White event</span> - event that was just triggered (< 1 sec)
<span style="color: yellow;">Yellow event</span> - event that was recently triggered (< 20 sec)
<span style="color: yellow;">Grey message</span> - event that hasn't triggered for a while (< 60 sec)

![screenshot](https://i.imgur.com/Z3zMOcJ.png)

## Installation
Extract the contents of the .zip into your KSP2 installation folder.

Mod folder will be placed in ..\Kerbal Space Program 2\BepInEx\plugins\

## How to use

- open the mod via app bar.
- see which event is being triggered by a certain user or game action
- to subscribe to the event use the following code in your mod:

~~~~~~~~
GameManager.Instance.Game.Messages.Subscribe<NameOfTheEventYouWantToSubscribeTo>(new Action<MessageCenterMessage>(this.YourMethodThatWillHandleTheEvent));

private void YourMethodThatWillHandleTheEvent(MessageCenterMessage obj)
{
	// Do stuff
} 
