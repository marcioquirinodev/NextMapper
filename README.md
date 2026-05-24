# NextMapper

O **NextMapper** é um mapeador de objetos de alta performance para .NET, construído sobre **Roslyn Source Generators**. Diferente de mapeadores tradicionais, ele não utiliza *Reflection* ou compilação dinâmica em tempo de execução, oferecendo um mapeamento de objetos tão rápido quanto código escrito manualmente (*zero-allocation*).

## 🚀 Por que NextMapper?

* **Performance Extrema:** Mapeamento executado em tempo de compilação, com custo de CPU próximo a zero.
* **Zero Alocação:** Sem *boxing/unboxing* ou instâncias dinâmicas.
* **Compile-time Safety:** Erros de mapeamento são capturados durante o *build*, não em produção.
* **Amigável ao Debugger:** O código gerado é C# puro; você pode usar o F11 do seu IDE para rastrear o mapeamento linha por linha.
* **Suporte a NativeAOT:** Totalmente compatível com compilações nativas.

## 🛠️ Funcionalidades Principais

* **Mapeamento Direto:** Convenção baseada em nomes idênticos.
* **Configuração Explícita:** Uso de atributos `[MapMember]` para nomes de propriedades distintos.
* **Achatamento Automático:** Suporte nativo a *Flattening* de objetos complexos (ex: `Address.City` -> `LocationCity`).
* **Tratamento de Coleções:** Conversão automática de listas e arrays via LINQ otimizado.

## 📋 Como usar

1. **Instale a biblioteca** no seu projeto (via NuGet local ou referência de projeto).
2. **Decore seu mapeador** com `[Mapper]`.
3. **Defina a assinatura** do método desejado:

```csharp
[Mapper]
public partial class CustomerMapper
{
    [MapMember(source: "LegalName", target: "DisplayName")]
    public partial CustomerViewModel MapToViewModel(Customer source);
}
