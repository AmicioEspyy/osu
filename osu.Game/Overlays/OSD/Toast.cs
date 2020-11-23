﻿// Copyright (c) ppy Pty Ltd <contact@ppy.sh>. Licensed under the MIT Licence.
// See the LICENCE file in the repository root for full licence text.

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using osu.Game.Input.Bindings;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Overlays.OSD
{
    public abstract class Toast : Container
    {
        private const int toast_minimum_width = 240;

        private readonly Container content;

        private readonly OsuSpriteText spriteText;

        private readonly string shortcut;

        private readonly GlobalAction? action;

        protected override Container<Drawable> Content => content;

        protected readonly OsuSpriteText ValueText;

        protected Toast(string description, string value, string shortcut = null, GlobalAction? action = null)
        {
            this.shortcut = shortcut;
            this.action = action;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            // A toast's height is decided (and transformed) by the containing OnScreenDisplay.
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

            InternalChildren = new Drawable[]
            {
                new Container // this container exists just to set a minimum width for the toast
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Width = toast_minimum_width
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.7f
                },
                content = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                },
                new OsuSpriteText
                {
                    Padding = new MarginPadding(10),
                    Name = "Description",
                    Font = OsuFont.GetFont(size: 14, weight: FontWeight.Bold),
                    Spacing = new Vector2(1, 0),
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Text = description.ToUpperInvariant()
                },
                ValueText = new OsuSpriteText
                {
                    Font = OsuFont.GetFont(size: 24, weight: FontWeight.Light),
                    Padding = new MarginPadding { Left = 10, Right = 10 },
                    Name = "Value",
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = value
                },
                spriteText = new OsuSpriteText
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    Name = "Shortcut",
                    Alpha = 0.3f,
                    Margin = new MarginPadding { Bottom = 15 },
                    Font = OsuFont.GetFont(size: 12, weight: FontWeight.Bold),
                },
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuConfigManager config)
        {
            string text;

            if (action != null)
                text = config.LookupKeyBindings(action);
            else if (!string.IsNullOrEmpty(shortcut))
                text = shortcut;
            else
                text = "no key bound";

            spriteText.Text = text.ToUpperInvariant();
        }
    }
}
