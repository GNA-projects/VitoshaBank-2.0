using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace VitoshaBank.Data.DbModels
{
    public partial class BankSystemContext : DbContext
    {
        public BankSystemContext()
        {
        }

        public BankSystemContext(DbContextOptions<BankSystemContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Cards> Cards { get; set; }
        public virtual DbSet<ChargeAccounts> Chargeaccounts { get; set; }
        public virtual DbSet<Credits> Credits { get; set; }
        public virtual DbSet<Deposits> Deposits { get; set; }
        public virtual DbSet<SupportTickets> Supporttickets { get; set; }
        public virtual DbSet<Transactions> Transactions { get; set; }
        public virtual DbSet<Users> Users { get; set; }
        public virtual DbSet<UserAccounts> Useraccounts { get; set; }
        public virtual DbSet<Wallets> Wallets { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "en_US.UTF-8");

            modelBuilder.Entity<Cards>(entity =>
            {
                entity.ToTable("cards", "vitosha");

                entity.HasIndex(e => e.CardNumber, "cards_card_number_key")
                    .IsUnique();

                entity.HasIndex(e => e.ChargeaccountId, "cards_chargeaccount_id_key")
                    .IsUnique();

                entity.HasIndex(e => e.Cvv, "cards_cvv_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("card_expiration_date");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("card_number");

                entity.Property(e => e.ChargeaccountId).HasColumnName("chargeaccount_id");

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("cvv");
            });

            modelBuilder.Entity<ChargeAccounts>(entity =>
            {
                entity.ToTable("chargeaccounts", "vitosha");

                entity.HasIndex(e => e.Iban, "chargeaccounts_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(6, 6)
                    .HasColumnName("amount");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(45)
                    .HasColumnName("iban");
            });

            modelBuilder.Entity<Credits>(entity =>
            {
                entity.ToTable("credits", "vitosha");

                entity.HasIndex(e => e.Iban, "credits_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(6, 6)
                    .HasColumnName("amount");

                entity.Property(e => e.CreditAmount)
                    .HasPrecision(6, 6)
                    .HasColumnName("credit_amount");

                entity.Property(e => e.CreditAmountLeft)
                    .HasPrecision(6, 6)
                    .HasColumnName("credit_amount_left");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("iban");

                entity.Property(e => e.Instalment)
                    .HasPrecision(6, 6)
                    .HasColumnName("instalment");

                entity.Property(e => e.Interest)
                    .HasPrecision(6, 6)
                    .HasColumnName("interest");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("payment_date");
            });

            modelBuilder.Entity<Deposits>(entity =>
            {
                entity.ToTable("deposits", "vitosha");

                entity.HasIndex(e => e.Iban, "deposits_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(6, 6)
                    .HasColumnName("amount");

                entity.Property(e => e.Divident)
                    .HasPrecision(6, 6)
                    .HasColumnName("divident");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("iban");

                entity.Property(e => e.PaymentDate)
                    .HasColumnType("date")
                    .HasColumnName("payment_date");

                entity.Property(e => e.TermOfPayment).HasColumnName("term_of_payment");
            });

            modelBuilder.Entity<SupportTickets>(entity =>
            {
                entity.ToTable("supporttickets", "vitosha");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Hasresponce).HasColumnName("hasresponce");

                entity.Property(e => e.Message)
                    .IsRequired()
                    .HasMaxLength(200)
                    .HasColumnName("message");

                entity.Property(e => e.Title)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("title");
            });

            modelBuilder.Entity<Transactions>(entity =>
            {
                entity.ToTable("transactions", "vitosha");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date).HasColumnName("date");

                entity.Property(e => e.Reason)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("reason");

                entity.Property(e => e.RecieverAccountInfo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("reciever_account_info");

                entity.Property(e => e.SenderAccountInfo)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("sender_account_info");

                entity.Property(e => e.TransactionAmount)
                    .HasPrecision(6, 6)
                    .HasColumnName("transaction_amount");
            });

            modelBuilder.Entity<Users>(entity =>
            {
                entity.ToTable("users", "vitosha");

                entity.HasIndex(e => e.Email, "users_email_key")
                    .IsUnique();

                entity.HasIndex(e => e.Username, "users_username_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ActivationCode)
                    .IsRequired()
                    .HasMaxLength(64)
                    .HasColumnName("activationcode");

                entity.Property(e => e.BirthDate)
                    .HasColumnType("date")
                    .HasColumnName("birth_date");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("email");

                entity.Property(e => e.FirstName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("first_name");

                entity.Property(e => e.IsAdmin).HasColumnName("isadmin");

                entity.Property(e => e.IsConfirmed).HasColumnName("isconfirmed");

                entity.Property(e => e.LastName)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("last_name");

                entity.Property(e => e.LastTransactionId).HasColumnName("last_transaction_id");

                entity.Property(e => e.Password)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("password");

                entity.Property(e => e.RegisterDate).HasColumnName("register_date");

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("username");
            });

            modelBuilder.Entity<UserAccounts>(entity =>
            {
                entity.ToTable("useraccounts", "vitosha");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.ChargeaccountId).HasColumnName("chargeaccount_id");

                entity.Property(e => e.CreditId).HasColumnName("credit_id");

                entity.Property(e => e.DepositId).HasColumnName("deposit_id");

                entity.Property(e => e.SupportId).HasColumnName("support_id");

                entity.Property(e => e.UserId).HasColumnName("user_id");

                entity.Property(e => e.UserUsername)
                    .HasMaxLength(60)
                    .HasColumnName("user_username");

                entity.Property(e => e.WalletId).HasColumnName("wallet_id");
            });

            modelBuilder.Entity<Wallets>(entity =>
            {
                entity.ToTable("wallets", "vitosha");

                entity.HasIndex(e => e.Amount, "wallets_amount_key")
                    .IsUnique();

                entity.HasIndex(e => e.CardNumber, "wallets_card_number_key")
                    .IsUnique();

                entity.HasIndex(e => e.Cvv, "wallets_cvv_key")
                    .IsUnique();

                entity.HasIndex(e => e.Iban, "wallets_iban_key")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Amount)
                    .HasPrecision(6, 6)
                    .HasColumnName("amount");

                entity.Property(e => e.CardExpirationDate)
                    .HasColumnType("date")
                    .HasColumnName("card_expiration_date");

                entity.Property(e => e.CardNumber)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("card_number");

                entity.Property(e => e.Cvv)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("cvv");

                entity.Property(e => e.Iban)
                    .IsRequired()
                    .HasMaxLength(60)
                    .HasColumnName("iban");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
