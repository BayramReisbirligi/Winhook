using ReisProduction.Winhook.Utilities.Enums;
using System.Runtime.CompilerServices;
using Windows.System;
namespace ReisProduction.Winhook.Models;
public partial class InputHook
{
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyDown(VirtualKey key)
    {
        InputDown?.Invoke((InputType)key);
        KeyDown?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyPress(VirtualKey key)
    {
        InputPress?.Invoke((InputType)key);
        KeyPress?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyDoublePress(VirtualKey key)
    {
        InputDoublePress?.Invoke((InputType)key);
        KeyDoublePress?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnKeyUp(VirtualKey key)
    {
        InputUp?.Invoke((InputType)key);
        KeyUp?.Invoke(key);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseDown(ButtonType button)
    {
        InputDown?.Invoke((InputType)button);
        MouseDown?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseClick(ButtonType button)
    {
        InputPress?.Invoke((InputType)button);
        MouseClick?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseHold(ButtonType button)
    {
        InputPress?.Invoke((InputType)button);
        MouseHold?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseDoubleClick(ButtonType button)
    {
        InputDoublePress?.Invoke((InputType)button);
        MouseDoubleClick?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseUp(ButtonType button)
    {
        InputUp?.Invoke((InputType)button);
        MouseUp?.Invoke(button);
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseScroll(ScrollType scroll)
    {
        MouseScroll?.Invoke(scroll);
        switch (scroll)
        {
            case ScrollType.MouseScrollUp:
                MouseScrollUp?.Invoke(scroll);
                break;
            case ScrollType.MouseScrollDown:
                MouseScrollDown?.Invoke(scroll);
                break;
            case ScrollType.MouseScrollLeft:
                MouseScrollLeft?.Invoke(scroll);
                break;
            case ScrollType.MouseScrollRight:
                MouseScrollRight?.Invoke(scroll);
                break;
        }
    }
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private void OnMouseMove(int deltaX, int deltaY)
    {
        MouseMove?.Invoke(deltaX, deltaY);
        if (deltaX is not 0)
        {
            MouseMoveHorizontal?.Invoke(deltaX, deltaY);
            if (deltaX > 0)
                MouseMoveRight?.Invoke(deltaX, deltaY);
            else
                MouseMoveLeft?.Invoke(deltaX, deltaY);
        }
        if (deltaY is not 0)
        {
            MouseMoveVertical?.Invoke(deltaX, deltaY);
            if (deltaY > 0)
                MouseMoveDown?.Invoke(deltaX, deltaY);
            else
                MouseMoveUp?.Invoke(deltaX, deltaY);
        }
    }
}