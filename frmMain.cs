﻿using System;
using System.Windows.Forms;
using Hansa.Model;
using Hansa.Utility;

namespace Hansa
{
    public partial class FrmMain : Form
    {
        public FrmMain()
        {
            InitializeComponent();
        }

        private void BtnD6_Click(object sender, EventArgs e)
        {
            var bracket = new Bracket();
            int.TryParse(txtLoad_D6.Text, out bracket.Load);
            bracket.Load /= 100; // kg→kN
            int.TryParse(txtArmLength_D6.Text, out bracket.ArmLength);
            int.TryParse(txtOD_D6.Text, out bracket.OD);
            int.TryParse(txtElevation_D6.Text, out bracket.Elevation);
            bracket.EndWelding = rioEndWelding_D6.Checked;
            if (bracket.Load <= 0 || bracket.ArmLength <= 0)
            {
                return;
            }
            new FrmD6(bracket).ShowDialog();
        }

        private void BtnD19_Click(object sender, EventArgs e)
        {
            var bracket = new Bracket();
            int.TryParse(txtLoad_D19.Text, out bracket.Load);
            bracket.Load /= 100;  // kg→kN
            int.TryParse(txtArmLength_D19.Text, out bracket.ArmLength);
            int.TryParse(txtOD_D19.Text, out bracket.OD);
            int.TryParse(txtElevation_D19.Text, out bracket.Elevation);
            bracket.EndWelding = rioEndWelding_D19.Checked;
            if (bracket.Load <= 0 || bracket.ArmLength <= 0)
            {
                return;
            }
            new FrmD19(bracket).ShowDialog();
        }

        private void BtnB1_1_Click(object sender, EventArgs e)
        {
            txtRod_B1_1.Clear();
            txtClamp_B1_1.Clear();
            txtRodLength_B1_1.Clear();
            var EL1 = Convert.ToInt32(txtEL1_B1_1.Text);
            var EL2 = Convert.ToInt32(txtEL2_B1_1.Text);
            var DN = cbxDN_B1_1.Text;

            string sql = string.Empty;
            if (rioBaseType_B1_1.Checked && rioBritishPipe_B1_1.Checked)
            {
                sql = $"SELECT * FROM B1_2a WHERE dn='{DN}'";
            }
            else if (rioBaseType_B1_1.Checked && !rioBritishPipe_B1_1.Checked)
            {
                sql = $"SELECT * FROM B1_2b WHERE dn='{DN}'";
            }
            else if (rioInsualationType1_B1_1.Checked && rioBritishPipe_B1_1.Checked)
            {
                sql = $"SELECT * FROM B1_3a WHERE dn='{DN}'";
            }
            else if (rioInsualationType1_B1_1.Checked && !rioBritishPipe_B1_1.Checked)
            {
                sql = $"SELECT * FROM B1_3b WHERE dn='{DN}'";
            }
            else if (rioInsualationType2_B1_1.Checked && rioBritishPipe_B1_1.Checked)
            {
                if (rioTempA_B1_1.Checked)
                {
                    sql = $"SELECT * FROM B1_4a WHERE clamp='DA-DN{DN}'";
                }
                if (rioTempB_B1_1.Checked)
                {
                    sql = $"SELECT * FROM B1_4a WHERE clamp='DB-DN{DN}'";
                }
                if (rioTempC_B1_1.Checked)
                {
                    sql = $"SELECT * FROM B1_4a WHERE clamp='DC-DN{DN}'";
                }
            }
            else if (rioInsualationType2_B1_1.Checked && !rioBritishPipe_B1_1.Checked)
            {
                if (rioTempA_B1_1.Checked)
                {
                    sql = $"SELECT * FROM B1_4b WHERE clamp='DA-DN{DN}'";
                }
                if (rioTempB_B1_1.Checked)
                {
                    sql = $"SELECT * FROM B1_4b WHERE clamp='DB-DN{DN}'";
                }
                if (rioTempC_B1_1.Checked)
                {
                    sql = $"SELECT * FROM B1_4b WHERE clamp='DC-DN{DN}'";
                }
            }

            // 指定管径
            var dt = SQLiteHelper.Read("Hansa.db", sql);
            var rod = Convert.ToString(dt.Rows[0]["rod"]);
            var clamp = Convert.ToString(dt.Rows[0]["clamp"]);
            var E = Convert.ToInt32(dt.Rows[0]["e"]);
            var load = Convert.ToInt32(dt.Rows[0]["load"]);

            // 指定吊杆
            if (chkRod_B1_1.Checked)
            {
                rod = cbxRod_B1_1.Text;
            }

            // 指定荷载
            var givenLoad = Convert.ToInt32(txtCheckLoad_B1_1.Text);
            if (chkCheckLoad_B1_1.Checked && givenLoad > load)
            {
                sql = sql.Substring(0, 26) + $"load > {givenLoad} LIMIT 0,1";
                dt = SQLiteHelper.Read("Hansa.db", sql);
                if (0 == dt.Rows.Count)
                {
                    MessageBox.Show("管道荷载过大，无法自动选型!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                rod = Convert.ToString(dt.Rows[0]["rod"]);
            }
            // M10~M20螺母高度与规格误差在4mm范围内
            var rodLength = EL1 - EL2 - E + Convert.ToInt32(rod.Substring(4, 2)) * 2.5;
            txtClamp_B1_1.Text = clamp;
            txtRod_B1_1.Text = rod;
            txtRodLength_B1_1.Text = Convert.ToString(rodLength);
            var type = rioBaseType_B1_1.Checked ? "I" : "II";

            Common.Copy2Clipboard($"B1-1\t{type}\t\t\t{EL1}\t{EL2}\t{rodLength}" +
                $"\t\t\t\t{E}\t\t\t1\t\t\t\t{rod}\t{clamp}\t\t\t\t1,1");
        }

        private void BtnB2_1_Click(object sender, EventArgs e)
        {
            txtLug_B2_1.Clear();
            txtRod_B2_1.Clear();
            txtClamp_B2_1.Clear();
            txtRodLength_B2_1.Clear();
            var EL1 = Convert.ToInt32(txtEL1_B2_1.Text);
            var EL2 = Convert.ToInt32(txtEL2_B2_1.Text);
            var DN = cbxDN_B2_1.Text;

            string sql = string.Empty;
            if (rioBaseType_B2_1.Checked && rioBritishPipe_B2_1.Checked)
            {
                sql = $"SELECT * FROM b2_2a WHERE dn='{DN}'";
            }
            else if (rioBaseType_B2_1.Checked && !rioBritishPipe_B2_1.Checked)
            {
                sql = $"SELECT * FROM b2_2b WHERE dn='{DN}'";
            }
            else if (rioInsualationType1_B2_1.Checked && rioBritishPipe_B2_1.Checked)
            {
                sql = $"SELECT * FROM b2_3a WHERE dn='{DN}'";
            }
            else if (rioInsualationType1_B2_1.Checked && !rioBritishPipe_B2_1.Checked)
            {
                sql = $"SELECT * FROM b2_3b WHERE dn='{DN}'";
            }
            else if (rioInsualationType2_B2_1.Checked && rioBritishPipe_B2_1.Checked)
            {
                if (rioTempA_B2_1.Checked)
                {
                    sql = $"SELECT * FROM b2_4a WHERE clamp='DA-DN{DN}'";
                }
                if (rioTempB_B2_1.Checked)
                {
                    sql = $"SELECT * FROM b2_4a WHERE clamp='DB-DN{DN}'";
                }
                if (rioTempC_B2_1.Checked)
                {
                    sql = $"SELECT * FROM b2_4a WHERE clamp='DC-DN{DN}'";
                }
            }
            else if (rioInsualationType2_B2_1.Checked && !rioBritishPipe_B2_1.Checked)
            {
                if (rioTempA_B2_1.Checked)
                {
                    sql = $"SELECT * FROM b2_4b WHERE clamp='DA-DN{DN}'";
                }
                if (rioTempB_B2_1.Checked)
                {
                    sql = $"SELECT * FROM b2_4b WHERE clamp='DB-DN{DN}'";
                }
                if (rioTempC_B2_1.Checked)
                {
                    sql = $"SELECT * FROM b2_4b WHERE clamp='DC-DN{DN}'";
                }
            }

            // 指定管径
            var dt = SQLiteHelper.Read("Hansa.db", sql);
            var lug = Convert.ToString(dt.Rows[0]["lug"]);
            var rod = Convert.ToString(dt.Rows[0]["rod"]);
            var clamp = Convert.ToString(dt.Rows[0]["clamp"]);
            var E = Convert.ToInt32(dt.Rows[0]["e"]);
            var F = Convert.ToInt32(dt.Rows[0]["f"]);
            var load = Convert.ToInt32(dt.Rows[0]["load"]);

            // 指定吊杆
            if (chkRod_B2_1.Checked)
            {
                sql = sql.Substring(0, 26) + $"rod='{cbxRod_B2_1.Text}'";
                dt = SQLiteHelper.Read("Hansa.db", sql);
                rod = Convert.ToString(dt.Rows[0]["rod"]);
                lug = Convert.ToString(dt.Rows[0]["lug"]);
                F = Convert.ToInt32(dt.Rows[0]["f"]);
            }

            // 指定荷载
            var givenLoad = Convert.ToInt32(txtCheckLoad_B2_1.Text);
            if (chkCheckLoad_B2_1.Checked && givenLoad > load)
            {
                sql = sql.Substring(0, 26) + $"load > {givenLoad} LIMIT 0,1";
                dt = SQLiteHelper.Read("Hansa.db", sql);
                if (0 == dt.Rows.Count)
                {
                    MessageBox.Show("管道荷载过大，无法自动选型!", "警告", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                rod = Convert.ToString(dt.Rows[0]["rod"]);
                lug = Convert.ToString(dt.Rows[0]["lug"]);
                F = Convert.ToInt32(dt.Rows[0]["f"]);
            }

            var rodLength = EL1 - EL2 - E - F;
            txtClamp_B2_1.Text = clamp;
            txtRod_B2_1.Text = rod;
            txtLug_B2_1.Text = lug;
            txtRodLength_B2_1.Text = Convert.ToString(rodLength);
            var type = rioBaseType_B2_1.Checked ? "I" : "II";

            Common.Copy2Clipboard($"B2-1\t{type}\t\t\t{EL1}\t{EL2}\t{rodLength}" +
                $"\t\t\t\t{E}\t{F}\t\t1\t\t\t{lug}\t{rod}\t{clamp}\t\t\t\t1,1,1");
        }


        private void BtnC7_1_Click(object sender, EventArgs e)
        {
            // 初始化清空
            txtLug_C7_1.Clear();
            txtSpring_C7_1.Clear();
            txtRod_C7_1.Clear();
            txtClamp_C7_1.Clear();
            txtLugLength_C7_1.Clear();
            txtSpringLength_C7_1.Clear();
            txtRodLength_C7_1.Clear();
            txtClampLength2_C7_1.Clear();

            // 获取界面数据
            var EL1 = Convert.ToInt32(txtEL1_C7_1.Text);
            var EL2 = Convert.ToInt32(txtEL2_C7_1.Text);
            var DN = cbxDN_C7_1.Text;
            var spring = cbxSpring_C7_1.Text;

            // 判断用哪个管夹表
            string clampTable = string.Empty;
            string clamp = string.Empty;
            // 基准型
            if (rioBaseType_C7_1.Checked && !rioBritishPipe_C7_1.Checked)
            {
                clampTable = "a5_1";
                clamp = $"A5-1({DN})";

            }
            else if (rioBaseType_C7_1.Checked && rioBritishPipe_C7_1.Checked)
            {
                clampTable = "a5_2";
                clamp = $"A5-2({DN})";
            }
            // 保温型
            else if (rioInsualationType1_C7_1.Checked && !rioBritishPipe_C7_1.Checked)
            {
                clampTable = "a7_1";
                clamp = $"A7-1({DN})";
            }
            else if (rioInsualationType1_C7_1.Checked && rioBritishPipe_C7_1.Checked)
            {
                clampTable = "a7_2";
                clamp = $"A7-2({DN})";
            }
            // 隔热型
            else if (rioInsualationType2_C7_1.Checked)
            {
                if (rioTempA_C7_1.Checked)
                {
                    clampTable = "da";
                    clamp = $"DA-DN{DN}";
                }
                if (rioTempB_C7_1.Checked)
                {
                    clampTable = "db";
                    clamp = $"DB-DN{DN}";
                }
                if (rioTempC_C7_1.Checked)
                {
                    clampTable = "dc";
                    clamp = $"DC-DN{DN}";
                }
            }

            // 指定管径
            var sql = $"SELECT * FROM {clampTable} WHERE clamp='{clamp}'";
            var dt = SQLiteHelper.Read("Hansa.db", sql);
            // 管夹长度
            var E = Convert.ToInt32(dt.Rows[0]["e"]);
            // 指定管夹长度
            if (cbxClampLength1_C7_1.Checked)
            {
                E = Convert.ToInt32(txtClampLength1_C7_1.Text);
            }
            sql = $"SELECT * FROM c7_1 WHERE spring='{spring}'";
            dt = SQLiteHelper.Read("Hansa.db", sql);
            var rod = Convert.ToString(dt.Rows[0]["rod"]);
            var lug = Convert.ToString(dt.Rows[0]["lug"]);
            var nut = Convert.ToString(dt.Rows[0]["d"]);

            // 吊耳长度
            var F = Convert.ToInt32(dt.Rows[0]["f"]);
            // 弹簧高度
            var H = Convert.ToInt32(dt.Rows[0]["h"]);
            // 伸入花篮螺母长度
            sql = $"SELECT * FROM orchid_bolt WHERE D='{nut}'";
            dt = SQLiteHelper.Read("Hansa.db", sql);
            var outLength = Convert.ToInt32(dt.Rows[0]["L"]) / 2 - Convert.ToInt32(dt.Rows[0]["h"]) / 2;
            // 计算吊杆长度
            var rodLength = EL1 - EL2 - E - F - H + outLength;
            // 更新界面
            txtLug_C7_1.Text = lug;
            txtSpring_C7_1.Text = spring;
            txtRod_C7_1.Text = rod;
            txtClamp_C7_1.Text = clamp;
            txtLugLength_C7_1.Text = F + string.Empty;
            txtSpringLength_C7_1.Text = H + string.Empty;
            txtRodLength_C7_1.Text = rodLength + string.Empty;
            txtClampLength2_C7_1.Text = E + string.Empty;

            var type = rioBaseType_C7_1.Checked ? "I" : "II";
            Common.Copy2Clipboard($"C7-1\t{type}\t\t\t{EL1}\t{EL2}\t{rodLength}" +
                $"\t\t\t\t{E}\t{F}\t{H}\t1\t\t\t{lug}\t{spring}\t{rod}\t{clamp}\t\t\t1,1,1,1");
        }

        private void BtnC8_Click(object sender, EventArgs e)
        {
            txtLug_C7_2.Clear();
            txtSpring_C7_2.Clear();
            txtRod_C7_2.Clear();
            txtLugLength_C7_2.Clear();
            txtSpringLength_C7_2.Clear();
            txtRodLength_C7_2.Clear();

            var EL1 = Convert.ToInt32(txtEL1_C7_2.Text);
            var EL2 = Convert.ToInt32(txtEL2_C7_2.Text);
            var steelHeight = Convert.ToInt32(txtSteelHeight_C7_2.Text);
            var spring = cbxSpring_C7_2.Text;

            var sql = $"SELECT * FROM c7_1 WHERE spring='{spring}'";
            var dt = SQLiteHelper.Read("Hansa.db", sql);
            var rod = Convert.ToString(dt.Rows[0]["rod"]);
            var lug = Convert.ToString(dt.Rows[0]["lug"]);
            var d = Convert.ToString(dt.Rows[0]["d"]);
            var F = Convert.ToInt32(dt.Rows[0]["f"]);
            var H = Convert.ToInt32(dt.Rows[0]["h"]);

            // 伸入花篮螺母长度
            sql = $"SELECT * FROM orchid_bolt WHERE D='{d}'";
            dt = SQLiteHelper.Read("Hansa.db", sql);
            var outLength1 = Convert.ToInt32(dt.Rows[0]["L"]) / 2 - Convert.ToInt32(dt.Rows[0]["h"]) / 2;

            // 下端外伸长度
            sql = $"SELECT * FROM nuts WHERE D='{d}'";
            dt = SQLiteHelper.Read("Hansa.db", sql);
            var outLength2 = Convert.ToDouble(dt.Rows[0]["m"]);
            outLength2 = Convert.ToInt32(outLength2 * 3);

            // TODO
            var rodLength = EL1 - EL2 - F - H + outLength1 + outLength2 + steelHeight;
            txtLug_C7_2.Text = lug;
            txtSpring_C7_2.Text = spring;
            txtRod_C7_2.Text = rod.Replace("A16", "A15");
            txtLugLength_C7_2.Text = F + string.Empty;
            txtSpringLength_C7_2.Text = H + string.Empty;
            txtRodLength_C7_2.Text = Convert.ToInt32(rodLength) + string.Empty;
        }

        private void TabMain_SelectedIndexChanged(object sender, EventArgs e)
        {
            var tab = sender as TabControl;
            switch (tab.SelectedIndex)
            {
                case 0:
                    AcceptButton = null;
                    break;
                case 1:
                    AcceptButton = BtnB1_1;
                    txtEL1_B1_1.Focus();
                    txtEL1_B1_1.SelectAll();
                    break;
                case 2:
                    AcceptButton = BtnB2_1;
                    txtEL1_B2_1.Focus();
                    txtEL1_B2_1.SelectAll();
                    break;
                case 3:
                    AcceptButton = BtnC7_1;
                    txtEL1_C7_1.Focus();
                    txtEL1_C7_1.SelectAll();
                    break;
                case 4:
                    AcceptButton = BtnC7_2;
                    txtEL1_C7_2.Focus();
                    txtEL1_C7_2.SelectAll();
                    break;
            }
        }

        private void RioInsualationType2_B1_1_CheckedChanged(object sender, EventArgs e)
        {
            grpTempRange_B1_1.Enabled = rioInsualationType2_B1_1.Checked;
        }

        private void RioInsualationType2_B2_1_CheckedChanged(object sender, EventArgs e)
        {
            grpTempRange_B2_1.Enabled = rioInsualationType2_B2_1.Checked;
        }

        private void RioInsualationType2_C7_1_CheckedChanged(object sender, EventArgs e)
        {
            grpTempRange_C7_1.Enabled = rioInsualationType2_C7_1.Checked;
        }

        private void ChkRod_B1_1_CheckedChanged(object sender, EventArgs e)
        {
            cbxRod_B1_1.Enabled = chkRod_B1_1.Checked;
        }

        private void ChkCheckLoad_B1_1_CheckedChanged(object sender, EventArgs e)
        {
            txtCheckLoad_B1_1.Enabled = chkCheckLoad_B1_1.Checked;
        }

        private void ChkCheckLoad_B2_1_CheckedChanged(object sender, EventArgs e)
        {
            txtCheckLoad_B2_1.Enabled = chkCheckLoad_B2_1.Checked;
        }

        private void ChkRod_B2_1_CheckedChanged(object sender, EventArgs e)
        {
            cbxRod_B2_1.Enabled = chkRod_B2_1.Checked;
        }

        private void CbxClampLength1_C7_1_CheckedChanged(object sender, EventArgs e)
        {
            txtClampLength1_C7_1.Enabled = cbxClampLength1_C7_1.Checked;
            rioBaseType_C7_1.Enabled = !cbxClampLength1_C7_1.Checked;
            rioInsualationType1_C7_1.Enabled = !cbxClampLength1_C7_1.Checked;
            rioInsualationType2_C7_1.Enabled = !cbxClampLength1_C7_1.Checked;
        }

        private void tabMain_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right && tabMain.SelectedTab == tabMain.TabPages[3])
            {
                //TODO
            }
        }
    }
}

