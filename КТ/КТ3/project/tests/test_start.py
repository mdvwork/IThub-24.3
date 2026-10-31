"""Один готовый пример. Он проходит и не доказывает исправность всей программы."""

import unittest

from mediapolka import issue, new_state


class StartTests(unittest.TestCase):
    def test_issue_one_microphone(self):
        state = new_state()
        result = issue(state, "mic", "1")

        self.assertTrue(result["ok"])
        self.assertEqual(state["stock"]["mic"], 2)
        self.assertEqual(len(state["loans"]), 1)
        self.assertEqual(state["loans"][0]["status"], "issued")
