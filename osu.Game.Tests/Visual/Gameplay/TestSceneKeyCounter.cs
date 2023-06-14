﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

#nullable disable

using System.Linq;
using NUnit.Framework;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Utils;
using osu.Game.Screens.Play;
using osu.Game.Screens.Play.HUD;
using osuTK;
using osuTK.Input;

namespace osu.Game.Tests.Visual.Gameplay
{
    [TestFixture]
    public partial class TestSceneKeyCounter : OsuManualInputManagerTestScene
    {
        [Cached]
        private readonly KeyCounterController controller;

        private readonly KeyCounterDisplay defaultDisplay;

        public TestSceneKeyCounter()
        {
            Children = new Drawable[]
            {
                controller = new KeyCounterController(),
                new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Direction = FillDirection.Vertical,
                    Spacing = new Vector2(72.7f),
                    Children = new[]
                    {
                        defaultDisplay = new DefaultKeyCounterDisplay
                        {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                        },
                        new ArgonKeyCounterDisplay
                        {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                        }
                    }
                }
            };

            controller.AddRange(new InputTrigger[]
            {
                new KeyCounterKeyboardTrigger(Key.X),
                new KeyCounterKeyboardTrigger(Key.X),
                new KeyCounterMouseTrigger(MouseButton.Left),
                new KeyCounterMouseTrigger(MouseButton.Right),
            });
        }

        [Test]
        public void TestDoThings()
        {
            var testCounter = (DefaultKeyCounter)defaultDisplay.Counters.First();

            AddStep("Add random", () =>
            {
                Key key = (Key)((int)Key.A + RNG.Next(26));
                controller.Add(new KeyCounterKeyboardTrigger(key));
            });

            Key testKey = ((KeyCounterKeyboardTrigger)controller.Triggers.First()).Key;

            addPressKeyStep();
            AddAssert($"Check {testKey} counter after keypress", () => testCounter.CountPresses.Value == 1);
            addPressKeyStep();
            AddAssert($"Check {testKey} counter after keypress", () => testCounter.CountPresses.Value == 2);
            AddStep("Disable counting", () => controller.IsCounting.Value = false);
            addPressKeyStep();
            AddAssert($"Check {testKey} count has not changed", () => testCounter.CountPresses.Value == 2);

            void addPressKeyStep() => AddStep($"Press {testKey} key", () => InputManager.Key(testKey));
        }
    }
}
