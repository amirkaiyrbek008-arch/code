import disnake
from disnake.ext import commands
from datetime import datetime, timedelta
import asyncio

intents = disnake.Intents.all()
zapretki = ["негр", "хуйло", "хуила", "хуйлан", "пидрила", "педик", "нигер", "пидар", "пидор", "пидарас", "сучка", "блядота", "шлюха", "сосал", "соси", "отчим", "чертила", "тварь", "нахуй", "ебало", "еблище", "пиздюк", "лох", "гитлер", "еблан", "даун", "дебил", "дибил", "шлюхи", "пизда", "ебал", "ебу", "сука"]
bot = commands.Bot(command_prefix="!", intents=intents, test_guilds=[1263516990300098581])
@bot.event
async def on_ready():
    print(f"{bot.user.name} запущен")

@bot.event
async def on_message(message):
    for slovo in message.content.split():
        for zapretka in zapretki:
            if slovo.lower() == zapretka:
                guild = bot.get_guild(1263516990300098581)
                mute_role = disnake.utils.get(guild.roles, id=1327325917118468197)
                channel_id = message.channel.id
                link = f"https://discord.com/channels/1263516990300098581/{channel_id}/{message.id}"
                mod_role = disnake.utils.get(guild.roles, id=1264970174600319068)
                modchan = disnake.utils.get(guild.text_channels, id=1327703609793581097)
                await modchan.send(f"{mod_role.mention}")
                embed = disnake.Embed(
                    title=f"Возможное нарушение",
                    description=(
                        f"**1. Имя Нарушителя:** {message.author.mention}\n"
                        f"**2. Ссылка на сообщение:** {link}"
                    ),
                    color=0x00dff7
                ) 

                await modchan.send(embed=embed)
    await bot.process_commands(message)

@bot.command()
@commands.has_permissions(kick_members=True)
async def kick(ctx, member: disnake.Member, *, reason=None):
    await member.kick(reason=reason)
    await ctx.send(f"{member.mention} был кикнут по пункту {reason}", delete_after=3)
    channel = disnake.utils.get(guild.text_channels, id=1264970942979903498)
    embed = disnake.Embed(
        title=f"Кик от Модератора",
        description=(
            f"**1. Имя Модератора:** {ctx.author.mention}\n"
            f"**2. Имя Нарушителя:** {member.mention}\n"
            f"**3. Нарушен пункт:** {reason}"
        ),
        color=0x00dff7
    )
    await channel.send(embed=embed)

@bot.command()
@commands.has_permissions(ban_members=True)
async def ban(ctx, member: disnake.Member, duration: int, *, reason=None):
    await member.ban(reason=reason)
    await ctx.send(f"{member.mention} был забанен по пункту {reason}", delete_after=3)
    channel = disnake.utils.get(ctx.guild.text_channels, id=1264970942979903498)
    embed = disnake.Embed(
        title=f"Бан от Модератора",
        description=(
            f"**1. Имя Модератора:** {ctx.author.mention}\n"
            f"**2. Имя Нарушителя:** {member.mention}\n"
            f"**3. Нарушен пункт:** {reason}"
        ),
        color=0x00dff7
    )
    await channel.send(embed=embed)

@bot.command()
@commands.has_permissions(ban_members=True)
async def unban(ctx, *, member_name):
    banned_users = await ctx.guild.bans()
    for ban_entry in banned_users:
        user = ban_entry.user
        if user.name == member_name:
            await ctx.guild.unban(user)
            await ctx.send(f"{member_name} был разбанен.", delete_after=3)
            return
            channel = disnake.utils.get(ctx.guild.text_channels, id=1264970942979903498)
            embed = disnake.Embed(
                title=f"Разбан от Модератора",
                description=(
                    f"**1. Имя Модератора:** {ctx.author.mention}\n"
                    f"**2. Имя Нарушителя:** {member.mention}"
                ),
                color=0x00dff7
            )

            await channel.send(embed=embed)
        else:
             await ctx.send(f"{member_name} не найден в списке банов.", delete_after=3)

@bot.command()
@commands.has_permissions(manage_roles=True)
async def mute(ctx, member: disnake.Member, duration: int, *, reason=None):
    mute_role = disnake.utils.get(ctx.guild.roles, id=1327325917118468197)
    if not mute_role:
        await ctx.send("Роль 'Мут' не найдена.", delete_after=3)
        return
    await member.add_roles(mute_role, reason=reason)
    await ctx.send(f"{member.mention} был замьючен на {duration} минут по пункту {reason}", delete_after=3)
    channel = disnake.utils.get(ctx.guild.text_channels, id=1264970942979903498)
    embed = disnake.Embed(
        title=f"Мут от Модератора",
        description=(
            f"**1. Имя Модератора:** {ctx.author.mention}\n"
            f"**2. Имя Нарушителя:** {member.mention}\n"
            f"**3. Нарушен пункт:** {reason}\n"
            f"**4. Длительность мута:** {duration} минут"
        ),
        color=0x00dff7
    )

    await channel.send(embed=embed)
    await asyncio.sleep(duration * 60)
    await member.remove_roles(mute_role)
    await ctx.send(f"Участник {member.mention} был автоматически размьючен.", delete_after=3)


@bot.command()
@commands.has_permissions(manage_roles=True)
async def unmute(ctx, member: disnake.Member):
    mute_role = disnake.utils.get(ctx.guild.roles, id=1327325917118468197)
    if mute_role in member.roles:
        await member.remove_roles(mute_role)
        await ctx.send(f"{member.mention} был размьючен.", delete_after=3)
        channel = disnake.utils.get(ctx.guild.text_channels, id=1264970942979903498)    
        embed = disnake.Embed(
            title=f"Размут от Модератора",
            description=(
                f"**1. Имя Модератора:** {ctx.author.mention}\n"
                f"**2. Имя Нарушителя:** {member.mention}"
            ),
            color=0x00dff7
        )
        await channel.send(embed=embed)
    else:
        await ctx.send(f"У {member.mention} нет мьюта.", delete_after=3)

@bot.command()
@commands.has_permissions(manage_roles=True)
async def warn(ctx, member: disnake.Member, numb, pos, reason=None):
    if numb != None and pos != None:
        if str(numb) == "1":
            warn_role = disnake.utils.get(ctx.guild.roles, id=1263583492495769681)
        elif str(numb) == "2":
            warn_role = disnake.utils.get(ctx.guild.roles, id=1263583541917388851)
        elif str(numb) == "3":
            warn_role = disnake.utils.get(ctx.guild.roles, id=1263583588163522632)
        else:
            await ctx.send("Вы ввели неверный аргумент после юзера игрока. Число должно быть от 1 до 3 включително в зависимости от номера варна", delete_after=10)

        if pos.lower() == "adm":
            channel = disnake.utils.get(ctx.guild.text_channels, id=1263528840681095269)
        elif pos.lower() == "mod":
            channel = disnake.utils.get(ctx.guild.text_channels, id=1284816932834381855)    
        elif pos.lower() == "bldr":
            channel = disnake.utils.get(ctx.guild.text_channels, id=1284826004782841876)    
        elif pos.lower() == "evn":
            channel = disnake.utils.get(ctx.guild.text_channels, id=1284823298206859315)        
        else:
            await ctx.send("Вы ввели неверный аргумент после номера варна. Аргумент должен содержать ADM/MOD/BLDR/EVN , в зависимости отдела", delete_after=10)
        await member.add_roles(warn_role)
        await ctx.send(f"Варн {numb} успешно был выдан игроку {member.mention}", delete_after=5)
        embed = disnake.Embed(
            title=f"Выдача варна",
            description=(
                f"**1. Имя игрока, который выдал варн:** {ctx.author.mention}\n"
                f"**2. Имя игрока, которому выдали варн:** {member.mention}\n"
                f"**3. Причина:** {reason}\n"
                f"**4. Номер варна:** {numb}"
            ),
            color=0x00dff7
        )
        await channel.send(embed=embed)
    else:
        await ctx.send("Вы не ввели некоторое аргументы", delete_after=5)

@bot.command()
@commands.has_permissions(manage_roles=True)
async def unwarn(ctx, member: disnake.Member, numb, pos, reason=None):
    if numb != None and pos != None:
        if str(numb) == "1":
            warn_role = disnake.utils.get(ctx.guild.roles, id=1263583492495769681)
        elif str(numb) == "2":
            warn_role = disnake.utils.get(ctx.guild.roles, id=1263583541917388851)
        elif str(numb) == "3":
            warn_role = disnake.utils.get(ctx.guild.roles, id=1263583588163522632)
        else:
            await ctx.send("Вы ввели неверный аргумент после юзера игрока. Число должно быть от 1 до 3 включително в зависимости от номера варна", delete_after=10)
        if warn_role not in member.roles:
            await ctx.send(f"У игрока нету варна {numb}")
        else:
            if pos.lower() == "adm":
                channel = disnake.utils.get(ctx.guild.text_channels, id=1263528840681095269)
            elif pos.lower() == "mod":
                channel = disnake.utils.get(ctx.guild.text_channels, id=1284816932834381855)    
            elif pos.lower() == "bldr":
                channel = disnake.utils.get(ctx.guild.text_channels, id=1284826004782841876)    
            elif pos.lower() == "evn":
                channel = disnake.utils.get(ctx.guild.text_channels, id=1284823298206859315)        
            else:
                await ctx.send("Вы ввели неверный аргумент после номера варна. Аргумент должен содержать ADM/MOD/BLDR/EVN , в зависимости отдела", delete_after=10)

            await member.remove_roles(warn_role)
            await ctx.send(f"У игрока {member.mention} был удален варн {numb}.", delete_after=3)
            embed = disnake.Embed(
                title=f"Удаление варна",
                description=(
                    f"**1. Имя игрока, который убрал варн:** {ctx.author.mention}\n"
                    f"**2. Имя игрока, которому выдали варн:** {member.mention}\n"
                    f"**3. Причина:** {reason}\n"
                    f"**4. Номер варна:** {numb}"
                ),
                color=0x00dff7
            )
            await channel.send(embed=embed)

@bot.command()
async def event(ctx, type, time, name):
    evn_role = disnake.utils.get(ctx.guild.roles, id=1265588453588992071)
    chan = disnake.utils.get(ctx.guild.text_channels, id=1266049714282041435)
    ping = disnake.utils.get(ctx.guild.roles, id=1266047247590752358)
    for i in ctx.author.roles:
        if i == evn_role:
            await chan.send(f"{ping.mention}")
            embed = disnake.Embed(
                title=f"Ивент",
                description=(

                    f"**1. Название ивента:** {name}\n"
                    f"**2. Тип ивента:** {type}\n"
                    f"**3. Ивентолог:** {ctx.author.mention}\n"
                    f"**4. Ивент начнется:** {time}"
                ),
                color=0x00dff7
            )
            await ctx.message.delete()
            await chan.send(embed=embed)
            break

@bot.command()
@commands.has_permissions(administrator=True)
async def spamdm(ctx, member: disnake.Member, duration: int, reason):
    if str(ctx.author.id) == "1139539667654557698" or str(ctx.author.id) == "553968819656327178":
        for i in range(duration):
            await member.send(f"{reason}")
    else:
        await ctx.send("ты не салел :rage:", delete_after=5)
@bot.event
async def on_command_error(ctx, error):
    if isinstance(error, commands.MissingPermissions):
        await ctx.send("У вас недостаточно прав.", delete_after=3)
    elif isinstance(error, commands.MissingRequiredArgument):
        await ctx.send("Вы забыли указать аргумент для команды.", delete_after=3)
    elif isinstance(error, commands.BadArgument):
        await ctx.send("Перепроверьте, корректно ли введены аргументы в поле. Если ошибка и дальше продолжается, то обратитесь к салрею(salrei3).", delete_after=3)
    else:
        await ctx.send("Произошла ошибка.", delete_after=3)

@bot.command()
@commands.has_permissions(administrator=True)
async def off(ctx, code):
    if str(code) == "123":
        if str(ctx.author.id) == "1139539667654557698" or str(ctx.author.id) == "553968819656327178":
            await bot.close()
        else:
            await ctx.send("ты не салел :rage:", delete_after=5)
bot.run("")
