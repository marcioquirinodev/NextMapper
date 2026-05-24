🚀 NextMapper

O NextMapper é um mapeador de objetos de altíssima performance para .NET, construído sobre a tecnologia moderna de Roslyn Source Generators.

Diferente de mapeadores tradicionais (como o AutoMapper clássico), o NextMapper não faz mágica em tempo de execução. Em vez disso, ele escreve o código de mapeamento para você de forma automatizada enquanto você compila o seu projeto. O resultado? Performance máxima, zero desperdício de memória e total facilidade para debugar.

💡 O que é um "Mapeador de Objetos"? (Explicação Simples)

Imagine que você tem uma ficha de cadastro no seu banco de dados chamada Customer (com informações sensíveis como senha criptografada, data de criação e auditoria). Na hora de enviar esses dados para a tela do seu aplicativo, você não quer (e não deve) enviar a ficha completa. Você cria uma ficha simplificada chamada CustomerViewModel ou CustomerDTO, contendo apenas o Nome e o E-mail.

O ato de copiar manualmente as informações de uma ficha para a outra é o que chamamos de Mapeamento de Objetos:

// Mapeamento manual (cansativo e repetitivo):
var viewModel = new CustomerViewModel();
viewModel.Name = customer.Name;
viewModel.Email = customer.Email;


Escrever isso para dezenas de tabelas e telas é exaustivo. É aí que o NextMapper entra: ele analisa as suas classes e escreve esse código repetitivo por você, de forma invisível e ultra-rápida!

🌟 Por que o NextMapper é diferente dos outros?

Os mapeadores antigos usam uma tecnologia chamada Reflection (Reflexão). Eles ficam inspecionando seus objetos em tempo de execução para descobrir quais propriedades existem. Isso deixa o aplicativo lento e consome muita memória.

O NextMapper é do Estado da Arte porque utiliza Source Generators:

Recurso

Mapeadores Comuns (Reflection)

NextMapper (Source Generator)

Velocidade

Média/Lenta (processa enquanto o app roda)

Instantânea (tão rápido quanto código feito à mão)

Uso de Memória

Alto (aloca recursos na memória Heap)

Zero Alocação Extra (otimização limpa)

Segurança

Se você errar um nome, o app quebra na mão do cliente

Se você errar um nome, o build falha na hora

Depuração (Debug)

É uma caixa preta (impossível debugar)

Totalmente transparente (você pode apertar F11)

🛠️ Funcionalidades Incríveis

Mapeamento por Convenção (Nomes Iguais): Se o seu modelo de origem e destino têm propriedades com o mesmo nome (ex: Id e Id), ele conecta automaticamente.

Mapeamento Explícito ([MapMember]): Se as propriedades têm nomes diferentes (ex: LegalName mapeia para DisplayName), você configura isso em um segundo usando atributos simples.

Achatamento Automático de Objetos (Flattening): Se o seu modelo tem um objeto dentro dele (ex: Customer.Location.City), o NextMapper descobre sozinho que ele deve mapear para LocationCity na sua tela simplificada.

Tratamento Inteligente de Coleções: Suporta cópias e conversões seguras de listas (List<T>) e arrays de forma automática.

📦 Estrutura do Projeto

Nosso repositório está organizado de forma limpa seguindo os padrões do mercado:

├── .gitignore
├── LICENSE
├── NextMapper.slnx          # Arquivo de solução moderno do VS 2026
├── README.md
└── src/
    ├── NextMapper.Abstractions/   # Contém os atributos leves ([Mapper], [MapMember])
    └── NextMapper.Engine/         # O motor gerador de código (Source Generator)


🚦 Como Instalar e Configurar no seu Projeto

Como os projetos do NextMapper estão dentro da pasta src/, veja como referenciá-los no projeto da sua aplicação (Web API, MVC, Console, etc.).

Abra o arquivo .csproj da sua aplicação e adicione as seguintes referências:

<ItemGroup>
  <!-- 1. Referência comum para o seu código enxergar os Atributos -->
  <ProjectReference Include="..\src\NextMapper.Abstractions\NextMapper.Abstractions.csproj" />

  <!-- 2. Referência especial para o compilador rodar o Motor Gerador -->
  <ProjectReference Include="..\src\NextMapper.Engine\NextMapper.Engine.csproj" 
                    OutputItemType="Analyzer" 
                    ReferenceOutputAssembly="false" />
</ItemGroup>


💡 Por que o Engine usa OutputItemType="Analyzer"?
Isso avisa ao compilador do .NET que o projeto Engine deve rodar exclusivamente durante a compilação para gerar o código, sem misturar ou poluir os arquivos executáveis finais do seu aplicativo.

💻 Guia Prático: Usando no Dia a Dia

Vamos construir um mapeamento completo do zero para você ver a mágica acontecer.

Passo 1: Crie as suas Classes (Modelos)

Vamos supor que você tem uma classe complexa de banco de dados (Customer) e deseja mapeá-la para uma classe de exibição (CustomerViewModel):

using System.Collections.Generic;

namespace MeuAplicativo.Models
{
    // Classe de Endereço Auxiliar
    public class Address 
    { 
        public string City { get; set; } 
        public string State { get; set; } 
    }

    // Modelo de Origem (Banco de Dados)
    public class Customer
    {
        public string LegalName { get; set; } // Nome diferente do destino
        public int ActiveYears { get; set; }    // Nome igual
        public Address Location { get; set; }  // Objeto complexo que será achatado
        public List<string> PhoneNumbers { get; set; } // Coleção de dados
    }

    // Modelo de Destino (Sua tela/ViewModel)
    public class CustomerViewModel
    {
        public string DisplayName { get; set; }  // Receberá 'LegalName'
        public int ActiveYears { get; set; }     // Receberá 'ActiveYears'
        public string LocationCity { get; set; } // Receberá automaticamente 'Location.City'
        public string LocationState { get; set; } // Receberá automaticamente 'Location.State'
        public List<string> PhoneNumbers { get; set; } // Cópia limpa da lista
    }
}


Passo 2: Crie a Classe Mapeadora

Agora, basta criar uma classe partial (parcial), decorá-la com o atributo [Mapper] e definir a assinatura do método desejado:

using NextMapper;
using MeuAplicativo.Models;

namespace MeuAplicativo.Mappers
{
    [Mapper]
    public partial class CustomerMapper
    {
        // Configuramos explicitamente que o 'LegalName' deve preencher o 'DisplayName'
        [MapMember(source: "LegalName", target: "DisplayName")]
        public partial CustomerViewModel MapToViewModel(Customer source);
    }
}


Passo 3: O que acontece quando você compila o projeto?

No momento em que você aperta Build/Compilar no seu IDE, o NextMapper entra em ação e gera silenciosamente este arquivo parcial nos bastidores:

// <auto-generated />
using System.Linq;

namespace MeuAplicativo.Mappers
{
    public partial class CustomerMapper
    {
        public partial MeuAplicativo.Models.CustomerViewModel MapToViewModel(MeuAplicativo.Models.Customer source)
        {
            if (source == null) return null;

            var target = new MeuAplicativo.Models.CustomerViewModel();

            // 1. Mapeamento Explícito resolvido com base no [MapMember]
            target.DisplayName = source.LegalName;

            // 2. Mapeamento Direto por convenção de nomes idênticos
            target.ActiveYears = source.ActiveYears;

            // 3. Tratamento de Coleções com Null-Safety
            target.PhoneNumbers = source.PhoneNumbers?.ToList();

            // 4. Achatamento de Objetos (Flattening) seguro contra erros de nulo (Null-Reference)
            if (source.Location != null)
                target.LocationCity = source.Location.City;

            if (source.Location != null)
                target.LocationState = source.Location.State;

            return target;
        }
    }
}


Passo 4: Como chamar o mapeador no seu código

No seu controlador de API ou serviço, basta instanciar a sua classe e chamar o método normalmente. Tudo funciona de forma simples, direta e extremamente rápida!

var mapper = new CustomerMapper();

var customer = new Customer
{
    LegalName = "Empresa de Software S.A.",
    ActiveYears = 5,
    Location = new Address { City = "Rio de Janeiro", State = "RJ" },
    PhoneNumbers = new List<string> { "21999999999" }
};

// Mapeando com facilidade!
CustomerViewModel viewModel = mapper.MapToViewModel(customer);

Console.WriteLine(viewModel.DisplayName);    // Saída: "Empresa de Software S.A."
Console.WriteLine(viewModel.LocationCity);   // Saída: "Rio de Janeiro"


❓ Perguntas Frequentes (FAQ)

1. Se uma propriedade existir no Model (Origem) mas não no ViewModel (Destino), vai dar erro?

Não! O NextMapper foca em satisfazer o objeto de Destino. Se a Origem tiver propriedades adicionais (como dados internos do banco de dados), elas serão simplesmente ignoradas de forma segura durante o mapeamento.

2. Esqueci de mapear uma propriedade obrigatória no meu ViewModel, o que acontece?

O NextMapper não gerará nenhuma linha de mapeamento para as propriedades que ele não conseguir resolver de forma direta ou automática. O valor inicial delas permanecerá o padrão do C# (null para textos, 0 para numéricos, false para booleanos, etc.), sem quebrar a execução do seu sistema.

3. Posso usar o comando "F11" (Step Into) para debugar o mapeamento?

Sim, absolutamente! Como o código é escrito de forma explícita pelo compilador, ele é C# comum. Você pode colocar um ponto de parada (breakpoint) na chamada do mapeamento e depurar linha por linha para entender exatamente como os seus dados estão fluindo.

📄 Licença

Este projeto é open-source e está licensed sob a licença MIT — o que significa que você é livre para usar, modificar, distribuir e integrar em qualquer projeto comercial sem restrições ou custos ocultos!

NextMapper — Performance em tempo de compilação para o ecossistema .NET.