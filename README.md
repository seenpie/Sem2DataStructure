# Sem2DataStructure

Репозиторий с решениями учебных задач по структурам данных и алгоритмам (семестр 2, C# / .NET 10).

[![.NET Build and Test](https://github.com/Eric-Cartmanez/Sem2DataStructure/actions/workflows/dotnet.yml/badge.svg)](https://github.com/Eric-Cartmanez/Sem2DataStructure/actions/workflows/dotnet.yml)
[![License: Unlicense](https://img.shields.io/badge/license-Unlicense-blue.svg)](https://unlicense.org)

---

## Быстрый старт

**Требования:** [.NET 10 SDK](https://dotnet.microsoft.com/download)

```bash
# Клонировать
git clone git@github.com:Eric-Cartmanez/Sem2DataStructure.git
cd Sem2DataStructure

# Собрать
dotnet build

# Запустить тесты
dotnet test

# Запустить интерактивное меню
dotnet run --project src/Runner
```

---

## Архитектура

```
Sem2DataStructure/
├── src/
│   ├── Tasks.Common/               # Общий контракт
│   │   ├── ISolution.cs            # Интерфейс ISolution { void Run(); }
│   │   └── TaskAttribute.cs        # Атрибут [Task(номер, название)]
│   ├── Runner/                     # Консольный Runner
│   │   └── Program.cs              # Меню, группировка по номеру задачи
│   └── Tasks/
│       ├── Task02.Nword/           # Задача 02: два решения
│       ├── Task03.BestTeacher/     # Задача 03
│       └── ...
├── tests/
│   └── Tasks.Tests/                # Все тесты (xUnit v3)
├── .github/workflows/dotnet.yml    # CI: сборка + тесты на каждый push
├── makeTask.sh                     # Скрипт-генератор заготовки новой задачи
└── LICENSE
```

### Как работает Runner

Runner при старте сканирует все `.dll` в своей директории, находит классы с атрибутом `[Task]`,
реализующие `ISolution`, группирует их по номеру задачи и выводит меню.

```
╔══════════════════════════════════════╗
║        Sem2DataStructure             ║
╚══════════════════════════════════════╝

Выберите задачу (0 — выход):

   1. [02] N-буквенные слова [2 решения]
   2. [03] Лучший преподаватель
   3. [04] Комбинированный массив
   ...

Задача: 1

=== [02] N-буквенные слова ===
Выберите решение:

  1. Рекурсивный
  2. Итеративный
```

Добавление новой задачи **не требует изменения Runner** — он обнаруживает её автоматически.

### Контракт задачи

Каждое решение состоит из двух уровней:

```csharp
// 1. Задача-специфичный интерфейс: чистая логика, тестируемая без консоли
public interface IMyTaskSolution : ISolution
{
    int Compute(int[] input);
}

// 2. Класс решения: I/O в Run(), логика в Compute()
[Task(8, "Название задачи")]
public class MyTaskSolution : IMyTaskSolution
{
    public void Run()
    {
        int[] input = ReadFromConsole();
        Console.WriteLine(Compute(input));
    }

    public int Compute(int[] input) { /* чистая логика */ }
}
```

Если у задачи несколько решений — третий аргумент атрибута задаёт название варианта:

```csharp
[Task(8, "Название задачи", "Быстрый")]
public class MyTaskFast : IMyTaskSolution { ... }

[Task(8, "Название задачи", "Простой")]
public class MyTaskSimple : IMyTaskSolution { ... }
```

---

## Как добавить новую задачу

### Способ 1 — автоматически (рекомендуется)

```bash
./makeTask.sh --number 8 --name MyTask
```

Скрипт сделает всё сам:
- создаст `src/Tasks/Task08.MyTask/` с интерфейсом, классом и `Task.md`
- создаст `tests/Tasks.Tests/Task08MyTaskTests.cs` с шаблоном тестов
- добавит проект в `Sem2DataStructure.slnx`, `Runner.csproj` и `Tasks.Tests.csproj`

После этого останется только заполнить логику и тесты.

---

### Способ 2 — вручную (шаг за шагом)

#### Шаг 1. Создать директорию задачи

Имя директории и проекта: `Task{NN}.{Name}`, где `NN` — двузначный номер.

```
src/Tasks/Task08.MyTask/
```

#### Шаг 2. Создать файл проекта `Task08.MyTask.csproj`

```xml
<Project Sdk="Microsoft.NET.Sdk">

  <PropertyGroup>
    <TargetFramework>net10.0</TargetFramework>
    <ImplicitUsings>enable</ImplicitUsings>
    <Nullable>enable</Nullable>
  </PropertyGroup>

  <ItemGroup>
    <ProjectReference Include="..\..\Tasks.Common\Tasks.Common.csproj" />
  </ItemGroup>

</Project>
```

#### Шаг 3. Создать интерфейс задачи `IMyTaskSolution.cs`

Добавьте методы с **чистой логикой** (без `Console.*`). Именно они будут тестироваться.

```csharp
using Tasks.Common;

namespace Task08.MyTask;

public interface IMyTaskSolution : ISolution
{
    // Метод с логикой задачи — без I/O, легко тестируется
    int Solve(int[] data);
}
```

#### Шаг 4. Создать класс решения `MyTaskSolution.cs`

```csharp
using Tasks.Common;

namespace Task08.MyTask;

[Task(8, "Название задачи")]   // номер определяет позицию в меню Runner'а
public class MyTaskSolution : IMyTaskSolution
{
    public void Run()
    {
        // Ввод данных из консоли
        int size = int.Parse(Console.ReadLine()!);
        var data = new int[size];
        for (int i = 0; i < size; i++)
            data[i] = int.Parse(Console.ReadLine()!);

        // Вызов логики и вывод результата
        Console.WriteLine(Solve(data));
    }

    public int Solve(int[] data)
    {
        // TODO: реализация
        throw new NotImplementedException();
    }
}
```

> Если у задачи **несколько решений**, создайте несколько классов с одним номером,
> но разными третьими аргументами атрибута:
>
> ```csharp
> [Task(8, "Название задачи", "Решение A")]
> public class MyTaskSolutionA : IMyTaskSolution { ... }
>
> [Task(8, "Название задачи", "Решение B")]
> public class MyTaskSolutionB : IMyTaskSolution { ... }
> ```

#### Шаг 5. Создать описание задачи `Task.md`

```markdown
# Задача 08: MyTask

Условие задачи...

**Пример:**
- Вход: `[1, 2, 3]`
- Выход: `6`

**Сложность:** O(n) по времени, O(1) по памяти.
```

#### Шаг 6. Добавить проект в решение и зависимости

```bash
# Добавить в solution
dotnet sln add src/Tasks/Task08.MyTask/Task08.MyTask.csproj

# Подключить к Runner (чтобы задача появилась в меню)
dotnet add src/Runner/Runner.csproj reference src/Tasks/Task08.MyTask/Task08.MyTask.csproj

# Подключить к тестам
dotnet add tests/Tasks.Tests/Tasks.Tests.csproj reference src/Tasks/Task08.MyTask/Task08.MyTask.csproj
```

#### Шаг 7. Написать тесты `tests/Tasks.Tests/Task08MyTaskTests.cs`

```csharp
using Task08.MyTask;

namespace Tasks.Tests;

public class Task08MyTaskTests
{
    // GetSolutions() позволяет автоматически прогонять все реализации
    // через одни и те же тесты — просто добавьте новое решение сюда.
    public static IEnumerable<object[]> GetSolutions()
    {
        yield return [new MyTaskSolution()];
        // yield return [new MyTaskSolutionB()];  // добавьте второе решение здесь
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void TaskExample(IMyTaskSolution s)
    {
        Assert.Equal(6, s.Solve([1, 2, 3]));
    }

    [Theory]
    [MemberData(nameof(GetSolutions))]
    public void EmptyInput(IMyTaskSolution s)
    {
        Assert.Equal(0, s.Solve([]));
    }
}
```

> **Совет:** тестируйте через интерфейс (`IMyTaskSolution`), а не через класс напрямую.
> Тогда при добавлении нового решения достаточно добавить его в `GetSolutions()` —
> все существующие тесты автоматически применятся к нему.

#### Шаг 8. Проверить

```bash
dotnet build
dotnet test
dotnet run --project src/Runner   # задача должна появиться в меню
```

---

## Подготовка решения к сдаче

Система автоматической проверки не использует наш `Runner`.
Она может ожидать либо **полноценную консольную программу** (читает ввод, пишет вывод), либо **только конкретную процедуру/функцию** — зависит от условия задачи. В любом случае нужно подготовить отдельный "чистый" файл.

**Наша архитектура с `Run()` предназначена для удобной локальной разработки, а для сдачи нужно подготовить "чистый" файл.**

Возьмите **чистую логику** из вашего метода (например, `Solve()`) и вставьте её в один из шаблонов ниже в зависимости от требований задачи.

**Вариант А — задача требует консольную программу** (читает ввод, выводит результат):

```csharp
// Правило 1: using System на месте
using System;

namespace MyTask
{
    internal class Program
    {
        // Правило 2: вся логика в статическом методе Main
        static void Main(string[] args)
        {
            // Правило 3: УДАЛИТЬ все подсказки для пользователя вида Console.Write("Введите...")
            // Система подаёт данные автоматически.
            string input = Console.ReadLine();

            // Чистая логика, скопированная из вашего метода Solve() / Compute() / ...
            // ...

            // Правило 4: вывести результат через Console.WriteLine()
            Console.WriteLine(result);
        }
    }
}
```

**Вариант Б — задача требует только процедуру/функцию** (без консольного ввода-вывода):

```csharp
using System;

namespace MyTask
{
    internal class Program
    {
        static void Main(string[] args) { }

        // Чистая логика, скопированная из вашего метода Solve() / Compute() / ...
        public static int Solve(int[] data)
        {
            // ...
        }
    }
}
```

### Чек-лист перед отправкой на проверку

- [ ] Код находится в структуре `namespace → class Program → static void Main`. Операторы верхнего уровня **не используются**.
- [ ] Прочитано условие: задача требует **консольную программу** или **только процедуру** — выбран соответствующий шаблон.
- [ ] Если требуется консольная программа: все подсказки пользователю (`Console.Write("Введите...")`, `Console.WriteLine("Результат:")` и т.п.) **полностью удалены**.
- [ ] Если требуется консольная программа: данные читаются через `Console.ReadLine()`, результат выводится через `Console.WriteLine()`.
- [ ] Формат вывода (разделители, регистр, переносы строк) соответствует требованиям задачи.
- [ ] Результат округлён **в точности** до того количества знаков, которое указано в условии.
- [ ] Код отлажен локально — `dotnet run` на тестовых данных из условия даёт верный ответ.
- [ ] Все тесты проходят: `dotnet test`.
- [ ] Вверху файла присутствует `using System;`.

---

## CI / GitHub Actions

Пайплайн запускается автоматически:
- при `push` в любую ветку, кроме `master`
- при `pull request` в `master`

Шаги: `restore` → `build --configuration Release` → `test`

Конфигурация: [`.github/workflows/dotnet.yml`](.github/workflows/dotnet.yml)

---

## Лицензия

[The Unlicense](LICENSE) — общественное достояние, делайте что хотите.
