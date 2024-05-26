partial class Creadf
{
    private void KeyPress(ConsoleKeyInfo KeyInfo)
    {
        // Ignore control characters other than the handled keybindings
        if (char.IsControl(KeyInfo.KeyChar))
            return;

        // Insert the character at the cursor position
        TextBuffer = TextBuffer.Insert(CursorVec.I, KeyInfo.KeyChar.ToString());

        // Update the positions
        CursorVec.I++;
        CursorVec.X++;
        CurrentSuggestionIdx = 0;

        UpdateBuffer();
    }

    private void HandleEnter()
    {
        if (Config.ToggleAutoComplete)
            ClearSuggestionBuffer();

        int TotalDist = Config.LeftCursorStartPos + TextBuffer.Length;
        (int _, int y) = CalcXYCordinate(TotalDist);
        y += CursorVec.Y;

        // Move the cursor to the end of the text
        CursorVec.X = 0;
        CursorVec.Y = y + 1;

        // Write a new line to output the text.
        Console.WriteLine();
        Loop = false;
    }

    // // Set the current suggesion in the text.
    // private void HandleCtrlEnter()
    // {
    //     TextBuffer += Suggestion;
    //     CursorVec.X += Suggestion.Length;
    //     CurrentSuggestionIdx = 0;
    //     UpdateBuffer(false);
    // }

    // // Render the next suggestion
    // private void HandleTab()
    // {
    //     if (ArrayIsEmpty([..Config.Suggestions]))
    //         return;

    //     CurrentSuggestionIdx = (CurrentSuggestionIdx + 1) % Config.Suggestions.Count;
    //     if (CurrentSuggestionIdx < 0 || CurrentSuggestionIdx > Config.Suggestions.Count)
    //         CurrentSuggestionIdx = 0;

    //     UpdateBuffer();
    // }

    // // Render the suggestions without typing anything
    // private void HandleCtrlSpacebar()
    // {
    //     if (!Config.ToggleAutoComplete)
    //         return;

    //     ClearSuggestionBuffer();
    //     RenderSuggestionBuffer();
    // }

    // // Clear all the suggestions
    // private void HandleEscape()
    // {
    //     if (!Config.ToggleAutoComplete)
    //         return;

    //     CurrentSuggestionIdx = 0;
    //     ClearSuggestionBuffer();
    // }

    // // Clear all the text
    // private void HandleShiftEscape()
    // {
    //     TextBuffer = "";
    //     CursorVec.X = Config.LeftCursorStartPos;
    //     CursorVec.Y = Config.TopCursorStartPos;
    //     CursorVec.I = 0;

    //     UpdateBuffer(false);
    // }

    private void HandleBackspace()
    {
        if (CursorVec.I > 0)
        {
            CursorVec.I--;
            CursorVec.X--;
            TextBuffer = TextBuffer.Remove(CursorVec.I, 1);

            if (CursorVec.X < 0)
            {
                CursorVec.X--;
                CursorVec.X += Console.WindowWidth;
                CursorVec.Y--;
            }

            UpdateBuffer();
        }
    }

    // private void HandleCtrlBackspace()
    // {
    //     if (CursorVec.I > 0)
    //     {
    //         if (TextBuffer.LastIndexOf(' ', CursorVec.X - Config.LeftCursorStartPos - 1) == CursorVec.X - Config.LeftCursorStartPos - 1)
    //             HandleBackspace();

    //         int PreviousWordIdx = TextBuffer.LastIndexOf(' ', CursorVec.X - Config.LeftCursorStartPos - 1);
    //         int length = CursorVec.X - Config.LeftCursorStartPos - PreviousWordIdx - 1;

    //         CursorVec.X -= length;
    //         CursorVec.I -= length;

    //         TextBuffer = TextBuffer.Remove(CursorVec.X - Config.LeftCursorStartPos, length);
    //         UpdateBuffer();
    //     }
    // }

    private void HandleDelete()
    {
        if (CursorVec.I < TextBuffer.Length)
        {
            TextBuffer = TextBuffer.Remove(CursorVec.X - Config.LeftCursorStartPos, 1);
            UpdateBuffer();
        }
    }

    // private void HandleCtrlDelete()
    // {
    //     if (CursorVec.I < TextBuffer.Length)
    //     {
    //         if (TextBuffer.IndexOf(' ', CursorVec.X - Config.LeftCursorStartPos) == CursorVec.X - Config.LeftCursorStartPos)
    //             HandleDelete();

    //         int NextWordIdx = TextBuffer.IndexOf(' ', CursorVec.X - Config.LeftCursorStartPos);
    //         int length = NextWordIdx == -1 ? TextBuffer.Length - (CursorVec.X - Config.LeftCursorStartPos) : NextWordIdx - (CursorVec.X - Config.LeftCursorStartPos);

    //         TextBuffer = TextBuffer.Remove(CursorVec.X - Config.LeftCursorStartPos, length);
    //         UpdateBuffer();
    //     }
    // }

    // private void HandleHome()
    // {
    //     CursorVec.X = Config.LeftCursorStartPos;
    //     CursorVec.Y = Config.TopCursorStartPos;
    //     CursorVec.I = 0;
    // }

    // private void HandleEnd()
    // {
    //     int TotalDist = Config.LeftCursorStartPos + TextBuffer.Length;
    //     int y = TotalDist / Console.WindowWidth;
    //     int x = TotalDist - (y * Console.WindowWidth);
    //     y += CursorVec.Y;

    //     // Move the cursor to the end of the text
    //     CursorVec.I = TextBuffer.Length;
    //     CursorVec.X = x;
    //     CursorVec.Y = y;
    // }

    private void HandleLeftArrow()
    {
        if (CursorVec.I > 0)
        {
            CursorVec.I--;
            CursorVec.X--;

            if (CursorVec.X < 0)
            {
                CursorVec.X--;
                CursorVec.X += Console.WindowWidth;
                CursorVec.Y--;
            }
        }
    }

    // private void HandleCtrlLeftArrow()
    // {
    //     if (CursorVec.I > 0)
    //     {
    //         if (TextBuffer.LastIndexOf(' ', CursorVec.I - 1) == CursorVec.I - 1)
    //         {
    //             CursorVec.X--;
    //             CursorVec.I--;
    //         }

    //         int PreviousWordIdx = TextBuffer.LastIndexOf(' ', CursorVec.I - 1);
    //         int length = CursorVec.I - PreviousWordIdx - 1;

    //         CursorVec.X -= length;
    //         CursorVec.I -= length;
    //     }
    // }

    private void HandleRightArrow()
    {
        if (CursorVec.I < TextBuffer.Length)
        {
            CursorVec.I++;
            CursorVec.X++;

            if (CursorVec.X >= Console.WindowWidth)
            {
                CursorVec.X = 1;
                CursorVec.Y++;
            }
        }
    }

    // private void HandleCtrlRightArrow()
    // {
    //     if (CursorVec.I < TextBuffer.Length)
    //     {
    //         if (TextBuffer.IndexOf(' ', CursorVec.X - Config.LeftCursorStartPos) == CursorVec.X - Config.LeftCursorStartPos)
    //         {
    //             CursorVec.X++;
    //             CursorVec.I++;
    //         }

    //         int NextWordIdx = TextBuffer.IndexOf(' ', CursorVec.X - Config.LeftCursorStartPos);
    //         int length = NextWordIdx == -1 ? TextBuffer.Length - (CursorVec.X - Config.LeftCursorStartPos) : NextWordIdx - (CursorVec.X - Config.LeftCursorStartPos);

    //         CursorVec.X += length;
    //         CursorVec.I += length;
    //     }
    // }
}