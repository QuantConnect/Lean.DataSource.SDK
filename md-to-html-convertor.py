def MdToHtml(lines):
    active = False
    container = False
    code = False
    table = False
    bullet = False
    attributes = False
    html = ""

    lines = [x.replace("\n", "") for x in lines if x.replace("\n", "") != ""]

    for line in lines:
        if "### " in line and "Meta" not in line:
            if table:
                table = False
                html += """</tbody>
</table>
"""

            if bullet:
                bullet = False
                html += """</ul>
"""

            if attributes:
                attributes = False

            if container:
                container = False
                html += """</div>
"""

            active = True

        if not active: continue

        if ("C#" in line or "Python" in line) and not container:
            container = True
            html += """<div class="section-example-container">
"""

        if "### " in line:
            html += f"""<h3>{line.replace('### ', '')}</h3>
"""

        elif container:
            if "Python" in line:
                html += f"""    <pre class="python">
"""

            elif "C#" in line:
                html += f"""    <pre class="csharp">
"""

            elif "```" in line:
                if not code:
                    code = True
                
                else:
                    code = False
                    html += """    </pre>
"""

            else:
                html += line + "\n"

        elif "- " not in line:
            processedString = line

            if "](http" in line:
                website = line.split("](")[-1].split(")")[0]
                processedString = line.replace("[", f'<a href="{website}">').replace(f"]({website})", "</a>")

            html += f"""<p>{processedString}</p>
"""

        elif "- **" in line:
            if not table:
                table = True
                html += """<table class="table qc-table">
<tbody>
"""

            key = line.split("**")[1]
            value = line.split(":")[-1].replace("\n", "").strip()
            html += f"""<tr><td>{key}</td><td>{value}</td></tr>
"""

        elif "- " in line:
            if not bullet:
                bullet = True
                html += """<ul>
"""

            point = line.replace('- ', '').replace('\n', '').strip()
            html += f"""<li>{point}</li>
"""

            if attributes:
                html += f"""<div data-tree="QuantConnect.DataSource.{point}"></div>
"""

        if "Data Point Attributes" in line:
            attributes = True

    if table:
        table = False
        html += """</tbody>
</table>
"""

    if bullet:
        bullet = False
        html += """</ul>
"""

    return html

with open("listing-about.md", "r", encoding="utf-8") as input:
    lines = input.readlines()

with open("listing-about.html", "w", encoding="utf-8") as html_file:
    html_file.write(MdToHtml(lines))

with open("listing-documentation.md", "r", encoding="utf-8") as input:
    lines = input.readlines()

with open("listing-documentation.html", "w", encoding="utf-8") as html_file:
    html_file.write(MdToHtml(lines))