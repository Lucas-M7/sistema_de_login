using System;
using System.Diagnostics.CodeAnalysis;
using BCrypt.Net;

Dictionary<string, string> usuarios = new Dictionary<string, string>();

Console.WriteLine("Bem-vindo(a) ao Sistema de Login!");
Console.WriteLine("---------------------------------");


while (true)
{
    Console.WriteLine("Escolha uma opção:");
    Console.WriteLine("1. Login");
    Console.WriteLine("2. Fazer Cadastro");
    Console.WriteLine("3. Sair");

    Console.WriteLine("Opcão: ");
    string opcao = Console.ReadLine();

    switch (opcao)
    {
        case "1":
            RealizarLogin();
            break;

        case "2":
            RealizarCadastro();
            break;

        case "3":
            Console.WriteLine("Obrigado por usar o Sistema de Login!");
            return;

        default:
            Console.WriteLine("Opção inválida. Tente Novamente.");
            break;
    }
}

void RealizarLogin()
{
    Console.WriteLine("Digite o seu nome de usuário: ");
    string inputUsuario = Console.ReadLine();

    Console.WriteLine("Digite a sua senha: ");
    string inputSenha = LerSenha();

    if (AutenticarUsuario(inputUsuario, inputSenha))
    {
        Console.WriteLine($"Login bem-sucessido! Bem-vindo(a), {inputUsuario} !");
        MenuLogado(inputUsuario);
    }
    else
    {
        Console.WriteLine("Falha no login. Verifique suas credenciais.");
    }
}

void RealizarCadastro()
{
    Console.WriteLine("Digite um nome de usuário: ");
    string novoUsuario = Console.ReadLine();

    //Verifica se o nome do usuário já existe
    if (usuarios.ContainsKey(novoUsuario))
    {
        Console.WriteLine("Nome de usuário já existe. Escolha outro.");
        return;
    }

    Console.WriteLine("Digite uma senha: ");
    string novaSenha = LerSenha();

    //Gera o salt único para cada usuário
    string salt = BCrypt.Net.BCrypt.GenerateSalt(12);

    //Gera o hash da senha com o salt
    string hashSenha = BCrypt.Net.BCrypt.HashPassword(novaSenha, salt);

    //Armazena o nome do usuário e o hash de senha
    usuarios.Add(novoUsuario, hashSenha);

    Console.WriteLine($"Conta criada com sucesso! Bem-Vindo, {novoUsuario}!");
    MenuLogado(novoUsuario);
}

void MenuLogado(string nomeUsuario)
{
    while (true)
    {
        Console.WriteLine("\nOpçoes disponíveis após o login: ");
        Console.WriteLine("1. Alterar senha: ");
        Console.WriteLine("2. Logout");
        Console.WriteLine("3. Sair");

        Console.WriteLine("Opção: ");
        string opcao = Console.ReadLine();

        switch (opcao)
        {
            case "1":
                AlterarSenha(nomeUsuario);
                break;
            case "2":
                Console.WriteLine("Logout realizado com sucesso!");
                return;
            case "3":
                Console.WriteLine("Obrigado por usar o Sistema de Login Seguro!");
                Environment.Exit(0);
                break;
            default:
                Console.WriteLine("Opção inválida. Tente novamente.");
                break;
        }
    }
}

void AlterarSenha(string nomeUsuario)
{
    Console.WriteLine("Digite sua nova senha: ");
    string novaSenha = LerSenha();

    string novoSalt = BCrypt.Net.BCrypt.GenerateSalt(12);
    string novoHashSenha = BCrypt.Net.BCrypt.HashPassword(novaSenha, novoSalt);

    usuarios[nomeUsuario] = novoHashSenha;

    Console.WriteLine("Senha alterada com sucesso!");
}

string LerSenha()
{
    string senha = "";
    ConsoleKeyInfo key;

    do
    {
        key = Console.ReadKey(true);

        //Se a tecla pressionada não for Enter, adiciona à senha
        if (key.Key != ConsoleKey.Enter)
        {
            senha += key.KeyChar;
            Console.WriteLine("*");
        }
    } while (key.Key != ConsoleKey.Enter);

    Console.WriteLine(); // Nova linha após a senha

    return senha;
}

bool AutenticarUsuario(string nomeUsuario, string senha)
{
    if (usuarios.ContainsKey(nomeUsuario))
    {
        string hashSenhaArmazenado = usuarios[nomeUsuario];

        //Gera o hash de senha de entrada usando o salt armazenado
        string hashSenhaInput = BCrypt.Net.BCrypt.HashPassword(senha, hashSenhaArmazenado);

        //Compara o hash da senha de entrada com o hash armazenado
        return hashSenhaInput == hashSenhaArmazenado;
    }

    return false;

}