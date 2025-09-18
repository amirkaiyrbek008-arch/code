import disnake
from disnake.ext import commands
from disnake.ui import Modal, TextInput
from disnake import TextInputStyle

intents = disnake.Intents.all()
bot = commands.Bot(command_prefix="!", intents=intents, test_guilds=[1263516990300098581])


Report_Channels = {
    "player": 1403042575669268602,
    "admin": 1403042639686668318,
    "management": 1403042689770983536,
    "donater": 1403042742048657459
}

Role_id = [1265371092902477895, 1271570756748312658, 1357388843434639602, 1379286284937465939, 1358045096767127774]

@bot.event
async def on_ready():
    print(f"{bot.user.name} запущен")
    channel = bot.get_channel(1403042222148026458)
    await channel.send("Выберите категорию жалобы:", view=ReportCategoryView())

class ReportModal(Modal):
    def __init__(self, category: str):
        self.category = category
        components = [
            TextInput(
                label="Сервер, на котором произошло нарушение",
                custom_id="server",
                placeholder="Название сервера",
                max_length=100,
                style=TextInputStyle.short,
            ),
            TextInput(
                label="Никнейм нарушителя",
                custom_id="offender",
                placeholder="Никнейм игрока, который нарушил",
                max_length=100,
                style=TextInputStyle.short,
            ),
            TextInput(
                label="Какое правило было нарушено",
                custom_id="violation",
                placeholder="Номер правил или краткое содержание ситуации",
                max_length=1000,
                style=TextInputStyle.paragraph,
            ),
            TextInput(
                label="Доказательства нарушения",
                custom_id="proof",
                placeholder="Видео MP4 или ссылка на YouTube",
                max_length=1000,
                style=TextInputStyle.paragraph,
            )
        ]
        super().__init__(title="Жалоба", custom_id=f"report_modal_{category}", components=components)

    async def callback(self, interaction: disnake.ModalInteraction):
        embed = disnake.Embed(title="Новая жалоба", color=disnake.Color.blue())
        embed.add_field(name="Категория", value=self.category.capitalize(), inline=False)
        embed.add_field(name="Сервер", value=interaction.text_values["server"], inline=False)
        embed.add_field(name="Нарушитель", value=interaction.text_values["offender"], inline=False)
        embed.add_field(name="Что было нарушено", value=interaction.text_values["violation"], inline=False)
        embed.add_field(name="Доказательства", value=interaction.text_values["proof"], inline=False)
        embed.set_footer(text=f"Отправил: {interaction.user} ({interaction.user.id})")

        view = disnake.ui.View()
        view.add_item(disnake.ui.Button(label="Одобрить", style=disnake.ButtonStyle.success, custom_id="approve_button"))
        view.add_item(disnake.ui.Button(label="Отклонить", style=disnake.ButtonStyle.danger, custom_id="reject_button"))
        view.add_item(disnake.ui.Button(label="На рассмотрении", style=disnake.ButtonStyle.blurple, custom_id="review_button"))

        channel = bot.get_channel(REPORT_CHANNELS.get(self.category))
        if channel:
            await interaction.user.send("Ваша жалоба успешно отправлена и находится на рассмотрении.")
            message = await channel.send(embed=embed, view=view)
            await message.create_thread(name=f"Жалоба от {interaction.user}")
            await interaction.response.send_message("Жалоба успешно отправлена!", ephemeral=True)
        else:
            await interaction.response.send_message("Ошибка: не найден канал для этой категории.", ephemeral=True)

class ReportCategoryView(disnake.ui.View):
    def __init__(self):
        super().__init__(timeout=None)
    @disnake.ui.button(label="Жалоба на игрока", style=disnake.ButtonStyle.red, custom_id="category_player")
    async def player_button(self, button, interaction):
        await interaction.response.send_modal(ReportModal("player"))

    @disnake.ui.button(label="Жалоба на админа", style=disnake.ButtonStyle.blurple, custom_id="category_admin")
    async def admin_button(self, button, interaction):
        await interaction.response.send_modal(ReportModal("admin"))

    @disnake.ui.button(label="Жалоба на руководство", style=disnake.ButtonStyle.green, custom_id="category_management")
    async def management_button(self, button, interaction):
        await interaction.response.send_modal(ReportModal("management"))

    @disnake.ui.button(label="Жалоба на донатера", style=disnake.ButtonStyle.grey, custom_id="category_donater")
    async def donater_button(self, button, interaction):
        await interaction.response.send_modal(ReportModal("donater"))

@bot.event
async def on_button_click(inter: disnake.MessageInteraction):
    if not any(role.id in Role_id for role in inter.author.roles) and not inter.author.guild_permissions.administrator:
        await inter.response.send_message("У вас нет прав для использования этой кнопки.", ephemeral=True)
        return
    message = inter.message
    old_embed = message.embeds[0]

    if inter.component.custom_id == "review_button":
        embed = disnake.Embed(
            title=old_embed.title,
            description=old_embed.description,
            color=disnake.Color.orange()
        )

        for field in old_embed.fields:
            if field.name not in ["Статус", "Причина"]:
                embed.add_field(name=field.name, value=field.value, inline=field.inline)

        embed.add_field(name="Статус", value=f"На рассмотрении. Жалобу рассматривает <@{inter.user.id}>", inline=False)

        await inter.response.send_message("Статус обновлён: на рассмотрении.", ephemeral=True)
        await message.edit(embed=embed)

    else:
        embed = disnake.Embed(
            title=old_embed.title,
            description=old_embed.description,
            color=disnake.Color.green() if inter.component.custom_id == "approve_button" else disnake.Color.red()
        )

        for field in old_embed.fields:
            embed.add_field(name=field.name, value=field.value, inline=field.inline)

        if inter.component.custom_id == "approve_button":
            embed.add_field(name="Статус", value="Одобрено", inline=False)
            embed.add_field(name="Причина", value=f"Жалоба принята <@{inter.user.id}>", inline=False)
            await inter.response.send_message("Жалоба одобрена", ephemeral=True)
        else:
            embed.add_field(name="Статус", value="Отклонено", inline=False)
            embed.add_field(name="Причина", value=f"Жалоба не соответствует критериям. Жалоба была рассмотрена <@{inter.user.id}>", inline=False)
            await inter.response.send_message("Жалоба отклонена", ephemeral=True)

        new_view = disnake.ui.View()
        for row in message.components:
            for component in row.children:
                new_view.add_item(disnake.ui.Button(
                    label=component.label,
                    style=component.style,
                    custom_id=component.custom_id,
                    disabled=True
                ))

        await message.edit(embed=embed, view=new_view)

bot.run("")
