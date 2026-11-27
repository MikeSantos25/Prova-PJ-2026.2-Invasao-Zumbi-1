using UnityEngine;
public static class CriadorDeSprites
{
    
    public static Sprite Municao()
    {
        Color borda = new Color(0.25f, 0.18f, 0.02f);
        Color fundo = new Color(0.95f, 0.75f, 0.1f);
        Color bala = new Color(0.55f, 0.35f, 0.05f);
        Texture2D t = NovaImagem();
        Preencher(t, Color.clear);
        Retangulo(t, 1, 3, 14, 10, borda);
        Retangulo(t, 2, 4, 12, 8, fundo);
       
        Retangulo(t, 4, 6, 2, 4, bala);
        Retangulo(t, 7, 6, 2, 4, bala);
        Retangulo(t, 10, 6, 2, 4, bala);
        return Finalizar(t);
    }
    public static Sprite KitMedico()
    {
        Color borda = new Color(0.3f, 0.3f, 0.3f);
        Color fundo = Color.white;
        Color cruz = new Color(0.9f, 0.1f, 0.1f);
        Texture2D t = NovaImagem();
        Preencher(t, Color.clear);
        Retangulo(t, 1, 2, 14, 12, borda);
        Retangulo(t, 2, 3, 12, 10, fundo);
        Retangulo(t, 7, 4, 2, 8, cruz);   
        Retangulo(t, 4, 7, 8, 2, cruz);   
        return Finalizar(t);
    }
    public static Sprite Bala()
    {
        Texture2D t = NovaImagem();
        Preencher(t, Color.clear);
        Retangulo(t, 6, 6, 4, 4, new Color(1f, 0.95f, 0.4f));
        Retangulo(t, 7, 7, 2, 2, Color.white);
        return Finalizar(t);
    }
    static Texture2D NovaImagem()
    {
        Texture2D t = new Texture2D(16, 16);
        t.filterMode = FilterMode.Point;   
        return t;
    }
    static void Preencher(Texture2D t, Color cor)
    {
        for (int x = 0; x < 16; x++)
            for (int y = 0; y < 16; y++)
                t.SetPixel(x, y, cor);
    }
    static void Retangulo(Texture2D t, int x0, int y0, int largura, int altura, Color cor)
    {
        for (int x = x0; x < x0 + largura; x++)
            for (int y = y0; y < y0 + altura; y++)
                t.SetPixel(x, y, cor);
    }

    static Sprite Finalizar(Texture2D t)
    {
        t.Apply();
        return Sprite.Create(t, new Rect(0, 0, 16, 16), new Vector2(0.5f, 0.5f), 32f);
    }
}
