import discord
from discord.ext import commands
from discord import app_commands
import requests
import datetime
import random
import string
def GenerateOrderId():
    chars = string.ascii_letters + string.digits
    return ''.join(random.choice(chars) for _ in range(8))
intents = discord.Intents.all()
intents.messages = True
intents.message_content = True
intents.guilds = True
from yoomoney import Quickpay
from yoomoney import Client
TOKEN = "4100115646854272.D77D90948298D62A52BE0CA7DAE8BDFDC07FB9FAAB0804B9E9CDE349D9B484C292C93FB39C2213A7112ADE13C6F3E49FA9043D77E8B5707927E867DDA7B6BF9080BEB0437A492DE20A1D030B7D95A39B3372AE4562CADA1BFCD901B4193EF3C3012840A3D46A1861B0969958F762DE72789CC205986351206D6DB8819748DC6B"
client = Client(TOKEN)
bot = commands.Bot(command_prefix="!", intents=intents)
def GetDonateCost(level):
        if level=="Уровень 1":
            return 100
        elif level == "Уровень 2":
            return 200
        elif level == "Уровень 3":
            return 300
        elif level == "Уровень 4":
            return 550
        elif level == "Уровень 5":
            return 600
        elif level == "Уровень 6":
            return 700
def GetDonateFromName(level):
    if level=="Уровень 1":
            return "lvl1"
    elif level == "Уровень 2":
            return "lvl2"
    elif level == "Уровень 3":
            return "lvl3"
    elif level == "Уровень 4":
            return "lvl4"
    elif level == "Уровень 5":
            return "lvl5"
    elif level == "Уровень 6":
            return "lvl6"
def CheckSteamId(steamid):
    if len(steamid) == 17:
        return True
    return False
def CreatePayment(cost, orderid):
    quickpay = Quickpay(
            receiver="4100115646854272", 
            quickpay_form="shop", 
            targets="Sponsor this project",
            paymentType="SB", 
            sum=cost, 
            label=orderid
            )
    return quickpay.redirected_url
def CheckPayment(orderid, price):
    history = client.operation_history(label=orderid)
    if history.operations == []:
        return False
    for operation in history.operations:
        if operation.status == 'success':
            return True
    return False
def GetDate30Days():
    today = datetime.date.today()
    future_date = today + datetime.timedelta(days=30)
    number = future_date.day + future_date.month * 30 + future_date.year * 365
    return number
def GetServerid(server):
    if server=="NoRules":
        return 0
    elif server=="Classic":
        return 1
def AddDonate(steamid, lvl, serverid):
    request = requests.get(f"http://mandarin.sensoft.pro/api/adddonate.php?steamid={steamid}&donate={GetDonateFromName(lvl)}&server={serverid}&date={GetDate30Days()}")
class TicketButtons(discord.ui.View):
    donatelevel = ""
    steamid = ""
    server = ""
    orderid =""
    def __init__(self):
        super().__init__(timeout=None)
    @discord.ui.select( 
        placeholder = "Уровень Доната", 
        min_values = 1, 
        max_values = 1, 
        options = [ 
            discord.SelectOption(
                label="Уровень 1",
                description="Уровень 1"
            ),
            discord.SelectOption(
                label="Уровень 2",
                description="Уровень 2"
            ),
            discord.SelectOption(
                label="Уровень 3",
                description="Уровень 3"
            ),
            discord.SelectOption(
                label="Уровень 4",
                description="Уровень 4"
            ),
            discord.SelectOption(
                label="Уровень 5",
                description="Уровень 5"
            ),
            discord.SelectOption(
                label="Уровень 6",
                description="Уровень 6"
            )
        ]
    )
    
    async def select_callback(self,interaction, select): 
        self.donatelevel = select.values[0]
        await interaction.response.send_message(f"Выбран {select.values[0]}!")
    @discord.ui.select( 
        placeholder = "Сервер", 
        min_values = 1, 
        max_values = 1, 
        options = [ 
            discord.SelectOption(
                label="NoRules",
                description="Мандариновый Комплекс [NoRules]"
            ),
            discord.SelectOption(
                label="Classic",
                description="Мандариновый Комплекс [Classic]"
            ),
            
        ]
    )
    
    async def second_select_callback(self,interaction, select): 
        self.server = select.values[0]
        await interaction.response.send_message(f"Выбран Сервер {self.server}!")
    @discord.ui.button(label="✅Оплатить",custom_id="payb", style=discord.ButtonStyle.success)
    async def first_button_callback(self,interaction, button):
        if self.donatelevel=="":
            await interaction.response.send_message(f"Ты Не Выбрал Уровень Доната")
        elif self.server=="":
            await interaction.response.send_message(f"Ты Не Выбрал Сервер")
        elif self.steamid=="":
            await interaction.response.send_message(f"Ты Не Ввел SteamId")
        else:
            embed = discord.Embed(title="Последняя Проверка", description=f"***Вы Уверены В Веденных Вами Данных?***\n**SteamId**: ``{self.steamid}``\n**Сервер**: ``{self.server}``\n**Донат**: ``{self.donatelevel}``\n***Для Подтверждения Введите 'Да' в чат.***", color=0xff0000)
            await interaction.channel.send(embed=embed)
            def check(msg):
                return msg.author == interaction.user and msg.channel == interaction.channel
            msg = await bot.wait_for('message', check=check, timeout=60)
            if (msg.content=="Да" or msg.content =="да"):
                self.orderid = GenerateOrderId()
                url = CreatePayment(GetDonateCost(self.donatelevel),self.orderid)
                await interaction.channel.send(f"Оплати {GetDonateCost(self.donatelevel)}₽ по ссылке [клик]({url}). После Оплаты нажмите на кнопку 'Проверить Оплату'")
            else:
                await interaction.channel.send("Введи Свои Данные Заново!")
            
    @discord.ui.button(label="🔵Проверить Оплату",custom_id="checkpayb", style=discord.ButtonStyle.success)
    async def check_button_callback(self,interaction, button):
        
        if CheckPayment(self.orderid, GetDonateCost(self.donatelevel)):
            embed = discord.Embed(title="Успех", description="Оплата Прошла Успешно\nПривилегия Была Выдана Вам На Аккаунт", color=0x1fff00)
            await interaction.response.send_message(embed=embed)
            AddDonate(self.steamid, self.donatelevel, GetServerid(self.server))
        else:
            embed = discord.Embed(title="Неуспешно", description="Оплата Не Прошла", color=0xff0000)
            await interaction.response.send_message(embed=embed)
        
            
        
    @discord.ui.button(label="💜Ввести SteamID",custom_id="steamidb", style=discord.ButtonStyle.success)
    async def input_button_callback(self,interaction, button):
        await interaction.response.send_message("Напишите свой SteamID64 в этот чат.")
        def check(msg):
            return msg.author == interaction.user and msg.channel == interaction.channel
        msg = await bot.wait_for('message', check=check, timeout=60)
        if (CheckSteamId(msg.content)):
            self.steamid = msg.content
            await interaction.channel.send("Успешно!")
        else:
            await interaction.channel.send("Вы Ввели неверный steamid!")

        
    @discord.ui.button(label="❌Закрыть",custom_id="closeb", style=discord.ButtonStyle.success)
    async def second_button_callback(self,interaction, button):
        channel = interaction.channel
        await channel.delete()
    @discord.ui.button(label="🟦Вызвать Поддержку",custom_id="supportb", style=discord.ButtonStyle.success)
    async def third_button_callback(self,interaction, button):
        await interaction.response.send_message(f"<@&1272182512923185285>")
class DonateButton(discord.ui.View):
    def __init__(self):
        super().__init__(timeout=None)
    @discord.ui.button(label="✅Купить!",custom_id="buyb", style=discord.ButtonStyle.success)
    async def button_callback(self,interaction, button):
        category = bot.get_channel(1272182158026608652)
        channel_name = interaction.user.name
        channel = await category.create_text_channel(channel_name)
        await channel.set_permissions(interaction.user, read_messages=True, send_messages=True)
        embed = discord.Embed(title="Покупка Доната", description="Чтобы Оплатить Донат - Нажмите на Кнопку Оплатить\nЧтобы Проверить Оплату - Нажмите Проверить Оплату\nЧтобы Ввести SteamId - Нажмите Ввести Steamid\nЧтобы Закрыть Тикет - Нажмите Закрыть\nЧтобы Вызвать Поддержку Нажмите - Вызвать Поддержку", color=0x1fff00)
        await interaction.response.send_message("Успешно Создал Тикет Для Вас", ephemeral=True)
        await channel.send(embed=embed, view = TicketButtons())
        await channel.send(f"<@{interaction.user.id}>")
        
@bot.event
async def on_ready():
    print(f'Logged in as {bot.user.name}')
    donatechannel = bot.get_channel(1263524793035133032)
    embed = discord.Embed(title="Покупка Доната", description="Чтобы Купить Донат Нажмите На Кнопку Ниже.", color=0x1fff00)
    await donatechannel.send(embed=embed, view = DonateButton())
def DaysToInt(day):
    today = datetime.date.today()
    future_date = today + datetime.timedelta(days=day)
    number = future_date.day + future_date.month * 30 + future_date.year * 365
    return number
def AddDonateDays(steamid, lvl, serverid,days):
    request = requests.get(f"http://mandarin.sensoft.pro/api/adddonate.php?steamid={steamid}&donate={lvl}&server={serverid}&date={days}")
@bot.command(name="givedonate",description='Выдать Донат Пользоватею')
async def givedonate(ctx,steamid:str, lvl:str, server:int, days: int):
    role = ctx.guild.get_role(1271931583854542859)
    if role in ctx.author.roles:
        AddDonateDays(steamid, lvl ,server, DaysToInt(days))
        await ctx.send(f'Донат Выдан Успешно!',ephemeral=True)
    else:
        await ctx.send('Нет Прав',ephemeral=True)    
    


bot.run('')
