import disnake
from disnake.ext import commands

bot = commands.InteractionBot()

# Простейшая функция для вычисления GPA
def predict_gpa(hours_study: float, credits: float, exam_score: float) -> float:
    gpa = 0.02 * hours_study + 0.03 * credits + 0.04 * exam_score
    return min(4.0, max(0.0, round(gpa, 2)))


# Определим форму для вопросов
class GPAModal(disnake.ui.Modal):
    def __init__(self):
        components = [
            disnake.ui.TextInput(
                label="Часы учебы в неделю",
                placeholder="Например: 15",
                custom_id="hours_study",
                style=disnake.TextInputStyle.short
            ),
            disnake.ui.TextInput(
                label="Количество кредитов",
                placeholder="Например: 30",
                custom_id="credits",
                style=disnake.TextInputStyle.short
            ),
            disnake.ui.TextInput(
                label="Средняя оценка экзамена (0-100)",
                placeholder="Например: 75",
                custom_id="exam_score",
                style=disnake.TextInputStyle.short
            )
        ]
        super().__init__(title="Предсказание GPA", components=components)

    async def callback(self, inter: disnake.ModalInteraction):
        try:
            hours_study = float(inter.text_values["hours_study"])
            credits = float(inter.text_values["credits"])
            exam_score = float(inter.text_values["exam_score"])

            predicted = predict_gpa(hours_study, credits, exam_score)

            await inter.response.send_message(
                f"📊 Предсказанный GPA: **{predicted}**",
                ephemeral=True
            )
        except ValueError:
            await inter.response.send_message(
                "❌ Введите корректные числовые значения!",
                ephemeral=True
            )


# Slash-команда для запуска модалки
@bot.slash_command(description="Предсказать GPA")
async def predict(inter: disnake.ApplicationCommandInteraction):
    await inter.response.send_modal(GPAModal())


bot.run("YOUR_BOT_TOKEN")