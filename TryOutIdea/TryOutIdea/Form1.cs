using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TryOutIdea
{
    // --------------------------------------------------------------------------------------------
    /// <!-- Form1 -->
    /// <summary>
    /// 
    /// </summary>
    public partial class Form1 : Form
    {
        // ----------------------------------------------------------------------------------------
        /// <!-- Form1 -->
        /// <summary>
        /// 
        /// </summary>
        public Form1()
        {
            InitializeComponent();
            btnRun.Text = "Test";
        }

        // ----------------------------------------------------------------------------------------
        /// <!-- btnRun_Click -->
        /// <summary>
        ///      basing L4 on L2
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRun_Click(object sender, EventArgs e)
        {
            // --------------------------------------------------------------------------
            // L4: (constructs at this level should be able to subsume the other levels)
            // --------------------------------------------------------------------------
            Graph     graph = new Graph();
            GraphNode node1 = new GraphNode("Jon");
            GraphNode node2 = new GraphNode("Grover");
            GraphEdge edge  = new GraphEdge(node1, node2);

            graph.Add(node1);
            graph.Add(node2);
            graph.Add(edge);
            string strEdge = edge.ToString();
            Assert.That(strEdge == "Jon -> Grover");


            // --------------------------------------------------------------------------
            // L3 concrete class: (there is no language, system or tool yet)
            // --------------------------------------------------------------------------
            MemberListConcrete dog = new MemberListConcrete("AnimalNamespace", "Dog");
            dog.Add("AnimalNamespace", "AppendageList", "Paws");
            dog.Add("AnimalNamespace", "Sense"        , "Nose");
            string strMemberList = dog.ToString();
            Assert.That(strMemberList  == "Dog : Paws, Nose");

            // --------------------------------------------------------------------------
            // L3 abstract class: (there is no language, system or tool yet)
            // --------------------------------------------------------------------------
            MemberList dog2 = new MemberList("AnimalNamespace", "Dog");



            // --------------------------------------------------------------------------
            // L2 concrete class: (oo languages)
            // --------------------------------------------------------------------------
            InheritClassConcrete inheritRelation = new InheritClassConcrete("Dog", "Animal");
            Assert.That(inheritRelation.ToString() == "Dog : Animal");
            inheritRelation = new InheritClassConcrete("AnimalNamespace", "Dog", "AnimalNamespace", "Animal");
            Assert.That(inheritRelation.ToString() == "AnimalNamespace.Dog : AnimalNamespace.Animal");

            // --------------------------------------------------------------------------
            // L2 abstract class: (oo languages) - basing L2 on L4
            // --------------------------------------------------------------------------
            InheritClass inheritRelation2 = new InheritClass("Dog", "Animal");
            Assert.That(inheritRelation2.ToString() == "Dog : Animal");
            inheritRelation2 = new InheritClass("AnimalNamespace", "Dog", "AnimalNamespace", "Animal");
            Assert.That(inheritRelation2.ToString() == "AnimalNamespace.Dog : AnimalNamespace.Animal");



            // --------------------------------------------------------------------------
            // L1 concrete class: (databases)
            // --------------------------------------------------------------------------
            ForeignKeyConcrete join = new ForeignKeyConcrete("EggDetail", "ParentEggID", "EggMaster", "EggID");
            string strJoin = join.ToString();
            Assert.That(strJoin == "EggDetail.ParentEggID >- EggMaster.EggID");

            // --------------------------------------------------------------------------
            // L1 abstract class: (databases) basing L1 on L4
            // --------------------------------------------------------------------------
            ForeignKey join2 = new ForeignKey("EggDetail", "ParentEggID", "EggMaster", "EggID");
            string strJoin2 = join2.ToString();
            Assert.That(strJoin2 == "EggDetail.ParentEggID >- EggMaster.EggID");



            btnRun.Text = "Success";
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    /// 
    /// </summary>
    public static class Assert
    {
        public static void That(bool ok)
        {
            if (!ok) throw new Exception("boom");
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    ///     native L4
    /// </summary>
    public class GraphNode // 
    {
        public string Container2 { get; set; }
        public string Container  { get; set; }
        public string Name       { get; set; }
        public string SourceNode { get; set; }


        // ----------------------------------------------------------------------------------------
        //  Constructor
        // ----------------------------------------------------------------------------------------
        public GraphNode(string name                  ) { Init(""       , name, name); }
        public GraphNode(string container, string name) { Init(container, name, name); }

        private void Init(string container, string name, string name_2)
        {
            SourceNode = name     ;
            Name       = name     ;
            Container  = container;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(Container)) return Name;
            else if (string.IsNullOrWhiteSpace(Container2)) return Container + "." + Name;
            else return Container2 + "." + Container + "." + Name;
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!-- Graph -->
    /// <summary>
    ///      native L4
    /// </summary>
    public class Graph // 
    {
        private List<GraphNode> _graph;
        private List<GraphEdge> _edge;

        public Graph()
        {
            _graph = new List<GraphNode>();
            _edge  = new List<GraphEdge>();
        }

        public void Add(GraphNode node)
        {
            _graph.Add(node);
        }

        public void Add(GraphEdge edge1)
        {
            _edge.Add(edge1);
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    /// 
    /// </summary>
    public class GraphEdge // native L4
    {
        public GraphNode SourceNode      { get; set; }
        public GraphNode DestinationNode { get; set; }
        public string    EdgeType        { get; set; }

        public GraphEdge(GraphNode fromNode, GraphNode toNode)
        {
            SourceNode      = fromNode;
            DestinationNode = toNode  ;
            EdgeType        = "Graph" ;
        }

        public GraphEdge(GraphNode FKColumn, GraphNode PKColumn, string edgeType)
        {
            SourceNode      = FKColumn;
            DestinationNode = PKColumn;
            EdgeType        = edgeType;
        }

        public string Connector { get
        {
            switch (EdgeType)
            {
                case "Foreign Key" : return " >- ";
                case "Inherits"    : return " : " ;
            }
            return " -> ";
        } }

        public override string ToString()
        {
            return SourceNode.ToString() + " -> " + DestinationNode.ToString();
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    /// 
    /// </summary>
    public class InheritClass
    {
        GraphNode MyClass;
        GraphNode ParentClass;
        GraphEdge Inheritance;

        public InheritClass(string name, string parent)
        {
            MyClass     = new GraphNode(name  );
            ParentClass = new GraphNode(parent);
            Inheritance = new GraphEdge(ParentClass, MyClass, "Inherits");
        }

        public InheritClass(string myNamespace, string className, string parentNamespace, string parentName)
        {
            MyClass     = new GraphNode(className ); MyClass.Container     = myNamespace    ;
            ParentClass = new GraphNode(parentName); ParentClass.Container = parentNamespace;
            Inheritance = new GraphEdge(ParentClass, MyClass, "Inherits");
        }

        public override string ToString()
        {
            return MyClass.ToString() + Inheritance.Connector + ParentClass.ToString();
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    /// 
    /// </summary>
    public class MemberList
    {
        GraphNode HeadOfList;

        public MemberList(string myNamespace, string className, string memberLabel)
        {
            HeadOfList = new GraphNode(memberLabel);
            HeadOfList.Container  = className;
            HeadOfList.Container2 = myNamespace;
        }

        public MemberList(string myNamespace, string className)
        {
            HeadOfList = new GraphNode("");
            HeadOfList.Container  = className;
            HeadOfList.Container2 = myNamespace;
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>members of a class - native L3</summary>
    public class MemberListConcrete
    {
        string MemberLabel    { get; set; }
        string ClassName      { get; set; }
        string ClassNamespace { get; set; }
        List<MemberListConcrete> LocalMember { get; set; }


        // ----------------------------------------------------------------------------------------
        //  Constructor
        // ----------------------------------------------------------------------------------------
        public MemberListConcrete(string myNamespace, string className                    ) { Init(myNamespace, className, ""         ); }
        public MemberListConcrete(string myNamespace, string className, string memberLabel) { Init(myNamespace, className, memberLabel); }

        private void Init(string myNamespace, string className, string memberLabel)
        {
            MemberLabel    = memberLabel;
            ClassNamespace = myNamespace;
            ClassName      = className  ;
            LocalMember    = new List<MemberListConcrete>();
        }

        public void Add(string memberClassNamespace, string memberClassName, string memberLabel)
        {
            LocalMember.Add(new MemberListConcrete(memberClassNamespace, memberClassName, memberLabel));
        }

        public override string ToString()
        {
            string members = "";
            string delim   = "";
            foreach (MemberListConcrete item in LocalMember)
            {
                members += (delim + item.MemberLabel);
                delim = ", ";
            }
            return ClassName + " : " + members;
        }
    }

    #region concrete classes

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    ///      native L2
    /// </summary>
    public class InheritClassConcrete //
    {
        string ClassName;
        string ClassNamespace;
        string ParentClassName;
        string ParentNamespace;

        public InheritClassConcrete(string name, string parent)
        {
            ClassName       = name  ;
            ParentClassName = parent;
        }

        public InheritClassConcrete(string myNamespace, string name, string parentNamespace, string parentName)
        {
            ClassNamespace  = myNamespace    ;
            ClassName       = name           ;
            ParentNamespace = parentNamespace;
            ParentClassName = parentName     ;
        }

        public override string ToString()
        {
            if (string.IsNullOrWhiteSpace(ClassNamespace) && string.IsNullOrWhiteSpace(ParentNamespace))
                return ClassName + " : " + ParentClassName;
            else return ClassNamespace + "." + ClassName + " : " + ParentNamespace + "." + ParentClassName;
        }
    }

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    ///      native L1
    /// </summary>
    public class ForeignKeyConcrete //
    {
        string DatabaseName  ;
        string SchemaName    ;
        string TableName     ;
        string ColumnLabel   ;

        string ToDatabaseName;
        string ToSchemaName  ;
        string ToTableName   ;
        string ToPrimaryKey  ;

        public ForeignKeyConcrete(string tableName, string columnLabel, string toTableName, string toPrimaryKey)
        {
            TableName    = tableName   ;
            ColumnLabel  = columnLabel ;
            ToTableName  = toTableName ;
            ToPrimaryKey = toPrimaryKey;
        }

        public override string ToString()
        {
            return TableName + "." + ColumnLabel + " >- " + ToTableName + "." + ToPrimaryKey;
        }
    }

    #endregion concrete classes

    // --------------------------------------------------------------------------------------------
    /// <!--  -->
    /// <summary>
    /// 
    /// </summary>
    public class ForeignKey
    {
        GraphNode ForeignTableColumn;
        GraphNode PrimaryTableColumn;
        GraphEdge FromForeignToPrimary;

        public ForeignKey(string tableName, string columnLabel, string toTableName, string toPrimaryKey)
        {
            ForeignTableColumn = new GraphNode(columnLabel);
            ForeignTableColumn.Container = tableName;

            PrimaryTableColumn = new GraphNode(toPrimaryKey);
            PrimaryTableColumn.Container = toTableName;

            FromForeignToPrimary = new GraphEdge(ForeignTableColumn, PrimaryTableColumn, "Foreign Key");
        }

        public override string ToString()
        {
            return ForeignTableColumn.ToString() + FromForeignToPrimary.Connector + PrimaryTableColumn.ToString();
        }
    }
}
