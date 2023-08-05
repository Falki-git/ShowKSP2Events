# ShowKSP2Events - a Kerbal Space Program 2 plugin
Shows events triggered by KSP2.

This is a mod that helps modders mod.

- **White event** - event that was just triggered (< 1 sec)
- **Yellow event** - event that was recently triggered (< 20 sec)
- **Gray event** - event that hasn't triggered for a while (< 60 sec)

![screenshot](https://i.imgur.com/oCfA2zs.png)

## Installation
Extract the contents of the .zip into your KSP2 installation folder.

Mod folder will be placed in ..\Kerbal Space Program 2\BepInEx\plugins\

## How to use

- open the mod via app bar.
- see which event is being triggered by a certain user or game action
- to subscribe to the event use the following code:

~~~~~~~~
// Place somewhere inside of: public override void OnInitialized()

var messages = GameManager.Instance.Game.Messages;
messages.Subscribe<NameOfTheEventYouWantToSubscribeTo>(new Action<MessageCenterMessage>(this.YourMethodThatWillHandleTheEvent));

private void YourMethodThatWillHandleTheEvent(MessageCenterMessage obj)
{
	// Do stuff
}
~~~~~~~~

## Other features
- pin events to the top
- ignore events you're not interested in
- log each triggering of an event to the KSP log
- get a list of all events that KSP2 can trigger
- customize timings of colors and purges
- export list to a JSON file inside mod folder
