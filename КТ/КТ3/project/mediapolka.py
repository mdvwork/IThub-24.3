"""Учебная версия учёта оборудования. Поведение нужно сверять с требованиями."""


def new_state():
    """Независимый комплект данных для запуска программы или теста."""
    return {
        "stock": {"mic": 3, "tripod": 1, "light": 0},
        "loans": [],
        "next_id": 1,
    }


def issue(state, item, quantity_text):
    """Выдать оборудование; quantity_text — строка из поля количества."""
    text = quantity_text.strip()
    if not text.isdecimal():
        return {"ok": False, "message": "Введите целое количество от 1 до 3"}

    quantity = int(text)
    if quantity < 1 or quantity >= 3:
        return {"ok": False, "message": "Введите целое количество от 1 до 3"}

    available = state["stock"][item]
    if available < 1:
        return {"ok": False, "message": "Недостаточно оборудования"}

    loan = {
        "id": state["next_id"],
        "item": item,
        "quantity": quantity,
        "status": "issued",
    }
    state["stock"][item] -= quantity
    state["loans"].append(loan)
    state["next_id"] += 1
    return {"ok": True, "message": "Выдано", "loan_id": loan["id"]}


def return_loan(state, loan_id):
    """Вернуть всё оборудование из одной записи выдачи."""
    for loan in state["loans"]:
        if loan["id"] == loan_id:
            state["stock"][loan["item"]] += loan["quantity"]
            loan["status"] = "returned"
            return {"ok": True, "message": "Возвращено"}
    return {"ok": False, "message": "Выдача не найдена"}
