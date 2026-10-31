"""Текстовый интерфейс. Учебные исправления делайте в mediapolka.py."""

from mediapolka import issue, new_state, return_loan

ITEMS = {"1": ("mic", "Микрофон"), "2": ("tripod", "Штатив"), "3": ("light", "LED-свет")}
NAMES = {code: name for code, name in ITEMS.values()}


def show(state):
    print("\nДоступное оборудование:")
    for key, (code, name) in ITEMS.items():
        print(f"  {key}. {name}: {state['stock'][code]}")
    print("Выдачи:")
    if not state["loans"]:
        print("  Пока нет")
    for loan in state["loans"]:
        status = "Выдано" if loan["status"] == "issued" else "Возвращено"
        print(f"  ID {loan['id']}: {NAMES[loan['item']]}, {loan['quantity']} шт., {status}")


def main():
    state = new_state()
    while True:
        show(state)
        command = input("\n1 — выдать; 2 — вернуть; 3 — сбросить данные; 0 — выйти: ").strip()
        if command == "0":
            break
        if command == "1":
            choice = input("Позиция (1/2/3): ").strip()
            if choice not in ITEMS:
                print("Выберите позицию 1, 2 или 3")
                continue
            result = issue(state, ITEMS[choice][0], input("Количество: "))
            print(result["message"])
        elif command == "2":
            try:
                loan_id = int(input("ID выдачи: "))
            except ValueError:
                print("Введите целый ID")
                continue
            print(return_loan(state, loan_id)["message"])
        elif command == "3":
            state = new_state()
            print("Учебные данные восстановлены")
        else:
            print("Выберите команду 0, 1, 2 или 3")


if __name__ == "__main__":
    try:
        main()
    except (EOFError, KeyboardInterrupt):
        print("\nРабота завершена")
