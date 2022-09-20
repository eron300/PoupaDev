Console.WriteLine("---- PoupaDev ----");

var objetivos = new List<ObjetivoFinanceiro> {
    new ObjetivoFinanceiro("Viagem a Orlando", 25000),
    new ObjetivoFinanceiroComPrazo(new DateTime(2023, 10, 1), "Viagem a Orlando com Prazo", 25000)
};

foreach (var objetivo in objetivos) {
    objetivo.ImprimirResumo();
}

ExibirMenu();

var opcao = Console.ReadLine();

while(opcao != "0") {
    switch (opcao) {
        case "1":
            // Cadastrar
            CadastrarObjetivo();
            break;
        case "2":
            // Depósito
            RealizarOperacao(TipoOperacao.Deposito);
            break;
        case "3":
            // Saque
            RealizarOperacao(TipoOperacao.Saque);
            break;
        case "4":
            // Detalhes
            ObterDetalhes();
            break;
        default:
            // Opção inválida.
            Console.WriteLine("Opção Inválida.");
            break;
    }

    ExibirMenu();

    opcao = Console.ReadLine();
}

void CadastrarObjetivo() {
    Console.WriteLine("Digite um título.");
    var titulo = Console.ReadLine();

    Console.WriteLine("Digite um valor de objetivo.");
    var valorLido = Console.ReadLine();
    var valor = decimal.Parse(valorLido);

    var objetivo = new ObjetivoFinanceiro(titulo, valor);

    objetivos.Add(objetivo);
    Console.WriteLine($"Objetivo ID: {objetivo.Id} foi criado com sucesso.");
}

void RealizarOperacao(TipoOperacao tipo) {
    Console.WriteLine("Digite o ID do Objetivo.");
    var idLido = Console.ReadLine();
    var id = int.Parse(idLido);

    Console.WriteLine("Digite o valor da operação:");
    var valorLido = Console.ReadLine();
    var valor = decimal.Parse(valorLido);

    var operacao = new Operacao(valor, tipo, id);

    var objetivo = objetivos.SingleOrDefault(o => o.Id == id);

    objetivo.Operacoes.Add(operacao);
}

void ObterDetalhes() {
    Console.WriteLine("Digite o ID do Objetivo.");
    var idLido = Console.ReadLine();
    var id = int.Parse(idLido);

    var objetivo = objetivos.SingleOrDefault(o => o.Id == id);

    objetivo.ImprimirResumo();
}

void ExibirMenu() {
    Console.WriteLine("Digite 1 para Cadastro de Objetivo.");
    Console.WriteLine("Digite 2 para Realizar um Depósito.");
    Console.WriteLine("Digite 3 para Realizar um Saque.");
    Console.WriteLine("Digite 4 para Exibir Detalhes de um Objetivo.");
    Console.WriteLine("Digite 0 para encerrar.");
}

// Enum permite que vc atribua uma palavra a um valor
public enum TipoOperacao {
    Saque = 0,
    Deposito = 1
}

public class Operacao {
    public Operacao(decimal valor, TipoOperacao tipo, int idObjetivo) {
        Id = new Random().Next(0, 1000);
        Valor = valor;
        Tipo = tipo;
        IdObjetivo = idObjetivo;
    }

    public int Id { get; private set; }
    public decimal Valor { get; private set; }
    public TipoOperacao Tipo { get; private set; }
    public int IdObjetivo { get; private set; }
}

public class ObjetivoFinanceiro {
    public ObjetivoFinanceiro(string? titulo, decimal? valorObjetivo) {
        Id = new Random().Next(0, 1000);
        Titulo = titulo;
        ValorObjetivo = valorObjetivo;

        Operacoes = new List<Operacao>();
    }

    public int Id { get; private set; }
    public string? Titulo { get; private set; }
    public decimal? ValorObjetivo { get; private set; }
    public List<Operacao> Operacoes { get; private set; }
    public decimal Saldo => ObterSaldo();

    public decimal ObterSaldo() {
        var totalDeposito = Operacoes
            // Pega todas as operações de um determinado tipo e soma apenas os valores
            .Where(o => o.Tipo == TipoOperacao.Deposito)
            .Sum(o => o.Valor);

        var totalSaque = Operacoes
            .Where(o => o.Tipo == TipoOperacao.Saque)
            .Sum(o => o.Valor);

        return totalDeposito - totalSaque;
    }

    public virtual void ImprimirResumo() {
        Console.WriteLine($"Objetivo {Titulo}, Valor: {ValorObjetivo}, com Saldo Atual: R${Saldo}");
    }
}

public class ObjetivoFinanceiroComPrazo : ObjetivoFinanceiro {
    public ObjetivoFinanceiroComPrazo(DateTime prazo, string? titulo, decimal? valorObjetivo) : base(titulo, valorObjetivo) {
        Prazo = prazo;
    }

    public DateTime Prazo { get; private set; }

    public override void ImprimirResumo() {
        base.ImprimirResumo();

        var diasRestantes = (Prazo - DateTime.Now).TotalDays;
        var valorRestante = ValorObjetivo - Saldo;

        Console.WriteLine($"Faltam {diasRestantes.ToString("F0")} para o prazo de seu objetivo, e faltam R${valorRestante} para completar.");
    }
}