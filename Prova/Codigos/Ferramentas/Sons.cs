using UnityEngine;

// ============================================================
// SONS DO JOGO
// ------------------------------------------------------------
// Este arquivo cuida de TODOS os sons. Você não precisa montar nada:
// ele cria os sons sozinho, por código (sem arquivos de áudio).
//
//   Sons.Tiro();               -> disparo da arma
//   Sons.Coleta();             -> pegar munição ou kit médico
//   Sons.PassoJogador();       -> passo do jogador
//   Sons.PassoZumbi(posicao);  -> passo de um zumbi (mais baixo quanto mais longe)
//
// COMO MUDAR O VOLUME:  mexa nos números logo abaixo (0 = mudo, 1 = máximo).
//
// COMO USAR SEUS PRÓPRIOS SONS (opcional):
//   1. Crie uma pasta chamada  Resources  dentro de Assets (se ainda não existir)
//      e, dentro dela, uma pasta chamada  Sons
//   2. Arraste seus arquivos de áudio para lá com ESTES nomes:
//        tiro   |   coleta   |   passo   |   passo_zumbi
//      (pode ser .wav ou .mp3. Não precisa mandar todos: o que faltar usa o som padrão.)
//   Pronto! Aperte Play.
// ============================================================
public static class Sons
{
    // ------------------------------------------------------------
    // VOLUMES  (pode mexer à vontade!)
    // ------------------------------------------------------------
    public const float VolumeTiro = 0.5f;
    public const float VolumeColeta = 0.6f;
    public const float VolumePassoJogador = 0.30f;
    public const float VolumePassoZumbi = 0.22f;
    public const float DistanciaMaximaDoPassoZumbi = 8f;    // zumbis mais longe que isso não fazem barulho
    public const float PausaEntrePassosDeZumbi = 0.20f;     // evita vários zumbis "metralhando" passos ao mesmo tempo

    // ------------------------------------------------------------
    // O que está guardado (você não precisa mexer daqui para baixo)
    // ------------------------------------------------------------
    const int Taxa = 22050;           // qualidade do som gerado

    static AudioSource fonte;         // o "alto-falante" do jogo
    static AudioClip[] tiros;
    static AudioClip[] coletas;
    static AudioClip[] passosJogador;
    static AudioClip[] passosZumbi;

    // ------------------------------------------------------------
    // OS COMANDOS (é isto que os outros códigos chamam)
    // ------------------------------------------------------------
    public static void Tiro()
    {
        Tocar(tiros, VolumeTiro);
    }

    public static void Coleta()
    {
        Tocar(coletas, VolumeColeta);
    }

    public static void PassoJogador()
    {
        Tocar(passosJogador, VolumePassoJogador);
    }

    static float ultimoPassoDeZumbi = -10f;

    public static void PassoZumbi(Vector3 posicao)
    {
        Camera camera = Camera.main;
        if (camera == null) return;

        // Com vários zumbis perto, só deixa tocar um passo de vez em quando (senão vira uma bagunça).
        if (Time.time - ultimoPassoDeZumbi < PausaEntrePassosDeZumbi) return;

        float distancia = Vector2.Distance(posicao, camera.transform.position);
        if (distancia >= DistanciaMaximaDoPassoZumbi) return;

        float proximidade = 1f - distancia / DistanciaMaximaDoPassoZumbi;   // 1 = pertinho, 0 = longe
        ultimoPassoDeZumbi = Time.time;
        Tocar(passosZumbi, VolumePassoZumbi * proximidade);
    }

    // ------------------------------------------------------------
    // TOCAR
    // ------------------------------------------------------------
    static void Tocar(AudioClip[] opcoes, float volume)
    {
        Preparar();
        if (opcoes == null || opcoes.Length == 0 || volume <= 0f) return;

        AudioClip som = opcoes[Random.Range(0, opcoes.Length)];
        if (som != null) fonte.PlayOneShot(som, volume);
    }

    // Cria o alto-falante e os sons na primeira vez que alguém pede um som.
    static void Preparar()
    {
        if (fonte != null) return;

        GameObject objeto = new GameObject("Sons");
        Object.DontDestroyOnLoad(objeto);          // continua existindo quando a fase reinicia
        fonte = objeto.AddComponent<AudioSource>();
        fonte.playOnAwake = false;
        fonte.spatialBlend = 0f;                   // som "de jogo 2D" (sem direção)

        if (tiros == null)
        {
            tiros = Opcoes("tiro", GerarTiro(1), GerarTiro(2), GerarTiro(3));
            coletas = Opcoes("coleta", GerarColeta());
            passosJogador = Opcoes("passo", GerarPasso(1, false), GerarPasso(2, false), GerarPasso(3, false));
            passosZumbi = Opcoes("passo_zumbi", GerarPasso(4, true), GerarPasso(5, true), GerarPasso(6, true));
        }
    }

    // Se existir um arquivo seu em Resources/Sons, usa ele. Senão, usa o som gerado.
    static AudioClip[] Opcoes(string nomeDoArquivo, params AudioClip[] padrao)
    {
        AudioClip seu = Resources.Load<AudioClip>("Sons/" + nomeDoArquivo);
        if (seu != null) return new AudioClip[] { seu };
        return padrao;
    }

    // ------------------------------------------------------------
    // SONS GERADOS POR CÓDIGO
    // ------------------------------------------------------------
    static AudioClip Criar(string nome, float[] amostras)
    {
        AudioClip clipe = AudioClip.Create(nome, amostras.Length, 1, Taxa, false);
        clipe.SetData(amostras, 0);
        return clipe;
    }

    // Tiro: um estalo de ruído + uma pancada grave que some rápido.
    static AudioClip GerarTiro(int semente)
    {
        System.Random sorteio = new System.Random(semente);
        float[] dados = new float[(int)(Taxa * 0.28f)];
        float fase = 0f;
        float suave = 0f;

        for (int i = 0; i < dados.Length; i++)
        {
            float t = i / (float)Taxa;
            float ruido = (float)(sorteio.NextDouble() * 2.0 - 1.0);
            suave += (ruido - suave) * 0.45f;                       // tira um pouco do chiado agudo

            float frequencia = 45f + 170f * Mathf.Exp(-t * 18f);    // a pancada começa aguda e desce
            fase += 2f * Mathf.PI * frequencia / Taxa;
            float pancada = Mathf.Sin(fase) * Mathf.Exp(-t * 13f);

            float estalo = (ruido * 0.4f + suave * 0.6f) * Mathf.Exp(-t * 26f);
            dados[i] = Mathf.Clamp(estalo * 0.9f + pancada * 0.8f, -1f, 1f);
        }
        return Criar("tiro" + semente, dados);
    }

    // Coleta: dois "plins" subindo (parecido com pegar uma moeda).
    static AudioClip GerarColeta()
    {
        float[] dados = new float[(int)(Taxa * 0.24f)];
        float fase = 0f;

        for (int i = 0; i < dados.Length; i++)
        {
            float t = i / (float)Taxa;
            bool segundaNota = t >= 0.08f;
            float frequencia = segundaNota ? 1320f : 880f;
            float tempoDaNota = segundaNota ? t - 0.08f : t;

            fase += 2f * Mathf.PI * frequencia / Taxa;
            float volume = Mathf.Exp(-tempoDaNota * 14f);
            dados[i] = (Mathf.Sin(fase) * 0.6f + Mathf.Sin(fase * 2f) * 0.15f) * volume;
        }
        return Criar("coleta", dados);
    }

    // Passo: um "chrec" curto de pisada em terra/folhas (ruído médio, SEM batida grave).
    // O do zumbi é mais abafado e um pouquinho mais longo (como se arrastasse o pé).
    static AudioClip GerarPasso(int semente, bool zumbi)
    {
        System.Random sorteio = new System.Random(semente);
        float duracao = zumbi ? 0.13f : 0.09f;
        float[] dados = new float[(int)(Taxa * duracao)];

        float cortaAgudos = (zumbi ? 0.12f : 0.28f) + semente * 0.005f;    // menor = som mais abafado
        float cortaGraves = 0.06f;                                        // tira o "tum" grave
        float agudos = 0f, graves = 0f;
        float rapidez = zumbi ? 34f : 55f;                                // quanto mais alto, mais curtinho

        for (int i = 0; i < dados.Length; i++)
        {
            float t = i / (float)Taxa;
            float ruido = (float)(sorteio.NextDouble() * 2.0 - 1.0);

            agudos += (ruido - agudos) * cortaAgudos;       // filtro que corta o chiado fino
            graves += (agudos - graves) * cortaGraves;      // acompanha só os graves...
            float medio = agudos - graves;                  // ...e tira eles: sobra só o "médio"

            float entrada = Mathf.Clamp01(t / 0.004f);      // começo suave (4 milésimos de segundo)
            dados[i] = medio * entrada * Mathf.Exp(-t * rapidez);
        }

        // Deixa todos os passos com a mesma força (o volume final é controlado lá em cima).
        float maior = 0.0001f;
        for (int i = 0; i < dados.Length; i++) maior = Mathf.Max(maior, Mathf.Abs(dados[i]));
        for (int i = 0; i < dados.Length; i++) dados[i] = dados[i] / maior * 0.9f;

        return Criar((zumbi ? "passoZumbi" : "passo") + semente, dados);
    }
}
