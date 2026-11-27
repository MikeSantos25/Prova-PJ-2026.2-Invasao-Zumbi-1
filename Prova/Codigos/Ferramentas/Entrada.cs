using UnityEngine;
using UnityEngine.InputSystem;

public static class Entrada
{
    public static Vector2 Movimento()
    {
        Keyboard teclado = Keyboard.current;
        if (teclado == null) return Vector2.zero;

        float x = 0f;
        float y = 0f;

        if (teclado.aKey.isPressed || teclado.leftArrowKey.isPressed) x -= 1f;
        if (teclado.dKey.isPressed || teclado.rightArrowKey.isPressed) x += 1f;
        if (teclado.sKey.isPressed || teclado.downArrowKey.isPressed) y -= 1f;
        if (teclado.wKey.isPressed || teclado.upArrowKey.isPressed) y += 1f;

        return new Vector2(x, y).normalized;
    }

    public static bool MouseAtirando()
    {
        return Mouse.current != null && Mouse.current.leftButton.isPressed;
    }

    public static bool EspacoApertado()
    {
        return Keyboard.current != null && Keyboard.current.spaceKey.isPressed;
    }

    public static Vector2 MouseNoMundo()
    {
        if (Mouse.current == null || Camera.main == null) return Vector2.zero;

        Vector3 posicaoNaTela = Mouse.current.position.ReadValue();
        Vector3 mundo = Camera.main.ScreenToWorldPoint(posicaoNaTela);
        return new Vector2(mundo.x, mundo.y);
    }

    public static bool ApertouEnter()
    {
        return Keyboard.current != null && Keyboard.current.enterKey.wasPressedThisFrame;
    }

    public static bool ApertouR()
    {
        return Keyboard.current != null && Keyboard.current.rKey.wasPressedThisFrame;
    }

    public static bool ApertouEsquerda()
    {
        Keyboard t = Keyboard.current;
        return t != null && (t.leftArrowKey.wasPressedThisFrame || t.aKey.wasPressedThisFrame || t.digit1Key.wasPressedThisFrame);
    }

    public static bool ApertouDireita()
    {
        Keyboard t = Keyboard.current;
        return t != null && (t.rightArrowKey.wasPressedThisFrame || t.dKey.wasPressedThisFrame || t.digit2Key.wasPressedThisFrame);
    }
}